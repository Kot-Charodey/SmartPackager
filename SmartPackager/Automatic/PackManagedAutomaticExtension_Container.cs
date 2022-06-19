using System;
using System.Linq;
using System.Reflection;

namespace SmartPackager.Automatic
{
    using ByteStack;

    internal static class PackManagedAutomaticExtension_Container
    {

        private const BindingFlags bindingFlagsDefault = BindingFlags.Public | BindingFlags.Instance;
        private const BindingFlags bindingFlagsAll = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private static bool IsStatic(this PropertyInfo property)
        {
            var g1 = property.GetGetMethod(false);
            var g2 = property.GetGetMethod(true);
            var s1 = property.GetSetMethod(false);
            var s2 = property.GetSetMethod(true);

            return
                (g1 != null && g1.IsStatic) || (g2.IsStatic) &&
                (s1 != null && s1.IsStatic) || (s2.IsStatic);

        }

        /// <summary>
        /// Создаёт упаковщик класса и управляймых структур (данный метод ищит все поля для упаковки и перадёт на упаковку)
        /// </summary>
        /// <typeparam name="TContainer">тип для которого будет создан упаковщик</typeparam>
        /// <returns>упаковщик</returns>
        public static PackManagedAutomatic<TContainer> Make<TContainer>(PackManagedAutomatic<TContainer> pma)
        {
            SearchPrivateFieldsAttribute osfAttribute = typeof(TContainer).GetCustomAttribute<SearchPrivateFieldsAttribute>();
            BindingFlags bindingFlags = osfAttribute == null ? bindingFlagsDefault : bindingFlagsAll;

            //ищет свойства и поля

            var fields =
                from field in typeof(TContainer).GetFields(bindingFlags)
                where !field.IsNotSerialized && !field.IsLiteral && !field.IsInitOnly && !field.IsStatic
                where field.GetCustomAttribute<NotPackAttribute>() == null
                where !field.Name.Contains("k__BackingField")
                select (MemberInfo)field;

            var properties =
                from property in typeof(TContainer).GetProperties(bindingFlags)
                where property.CanWrite && property.CanRead
                where (property.GetGetMethod(false)) == null
                where property.GetCustomAttribute<NotPackAttribute>() == null
                where !property.IsStatic()
                select (MemberInfo)property;

            MemberInfo[] memberInfo = (
                from member in fields.Union(properties)
                orderby member.Name ascending
                select member).ToArray();


            if (memberInfo.Length == 0)
            {
                throw new Exception("You cant't pack a void!");
            }


            Container.Pack(pma, memberInfo);
            return pma;
        }

        private static class Container
        {
            private static MethodInfo PackExtension_MethodInfo => typeof(Container).GetMethod(nameof(Container.PackExtension), MethodUtil.BindingFind);
            private static MethodInfo CreateClass_MethodInfo => typeof(Container).GetMethod(nameof(Container.CreateClass), MethodUtil.BindingFind);

            private delegate T Delegate_CreateClass<T>();
            private delegate bool Delegate_PackExtension<TContainer>(PackManagedAutomatic<TContainer> pma, MemberInfo memberInfo);

            /// <summary>
            /// Инициализирует упаковщик
            /// </summary>
            /// <typeparam name="TContainer">тип для которого будет создан упаковщик</typeparam>
            /// <param name="pma">упаковщик который необходимо инициализировать</param>
            /// <param name="membersInfo">поля которые необходимо упаковывать</param>
            public static void Pack<TContainer>(PackManagedAutomatic<TContainer> pma, MemberInfo[] membersInfo)
            {
                pma.IsFixedSize = false;
                bool inLocalFixedSize = true;

                //заполняет укаковщики полей + устанавливает флаг - все ли поля фиксированной длинны
                for (int i = 0; i < membersInfo.Length; i++)
                {
                    var typeTContainer = typeof(TContainer);
                    var memberInfo = membersInfo[i];
                    var typeTField = memberInfo.GetUnderlyingType();

                    var packExtension = PackExtension_MethodInfo.MakeGenericDelegate<Delegate_PackExtension<TContainer>>(typeTContainer, typeTField);
                    inLocalFixedSize &= packExtension(pma, memberInfo);
                }

                //инициализирует основной упаковщик контейнера

                //если это структура
                if (typeof(TContainer).IsValueType)
                {
                    void PackUp(ref StackWriter writer, TContainer source, GenericPackUP<TContainer> packUP)
                    {
                        packUP(ref writer, source);
                    }

                    void UnPack(ref StackReader reader, out TContainer destination, GenericUnPack<TContainer> unPack)
                    {
                        destination = default;
                        unPack(ref reader, ref destination);
                    }

                    //если все локальные полня фиксированного размера - оптимизируем расчёт размера
                    if (inLocalFixedSize)
                    {
                        var meterLocal = new StackMeter();
                        pma.MembersGetSize(ref meterLocal, default);
                        int len = meterLocal.GetCalcLength();

                        void GetSize(ref StackMeter meter, TContainer source, GenericGetSize<TContainer> getSize)
                        {
                            meter.AddFixedSize(len);
                        }

                        pma.MainGetSize = GetSize;
                    }
                    else
                    {
                        void GetSize(ref StackMeter meter, TContainer source, GenericGetSize<TContainer> getSize)
                        {
                            getSize(ref meter, source);
                        }

                        pma.MainGetSize = GetSize;
                    }

                    pma.MainPackUp = PackUp;
                    pma.MainUnPack = UnPack;
                }
                else //если это класс (ссылочный тип)
                {
                    var createClass = CreateClass_MethodInfo.MakeGenericDelegate<Delegate_CreateClass<TContainer>>();

                    void PackUp(ref StackWriter writer, TContainer source, GenericPackUP<TContainer> packUP)
                    {
                        if (writer.MakeReference(source))
                            packUP(ref writer, source);
                    }

                    void UnPack(ref StackReader reader, out TContainer destination, GenericUnPack<TContainer> unPack)
                    {
                        if (reader.ReadReference())
                        {
                            destination = reader.GetReferenceObject<TContainer>();
                        }
                        else
                        {
                            destination = createClass();
                            reader.AttachReference(destination);
                            unPack(ref reader, ref destination);
                        }
                    }

                    //если все локальные полня фиксированного размера - оптимизируем расчёт размера
                    if (inLocalFixedSize)
                    {
                        var meterLocal = new StackMeter();
                        pma.MembersGetSize(ref meterLocal, createClass());
                        int len = meterLocal.GetCalcLength();

                        void GetSize(ref StackMeter meter, TContainer source, GenericGetSize<TContainer> getSize)
                        {
                            if (meter.MakeReference(source))
                            {
                                meter.AddFixedSize(len);
                            }
                        }

                        pma.MainGetSize = GetSize;
                    }
                    else
                    {
                        void GetSize(ref StackMeter meter, TContainer source, GenericGetSize<TContainer> getSize)
                        {
                            if (meter.MakeReference(source))
                            {
                                getSize(ref meter, source);
                            }
                        }

                        pma.MainGetSize = GetSize;
                    }



                    pma.MainPackUp = PackUp;
                    pma.MainUnPack = UnPack;
                }
            }

            /// <summary>
            /// Создаёт копию класа (пустой конструктор)
            /// </summary>
            /// <typeparam name="T">тип который надо создать</typeparam>
            /// <returns></returns>
            private static T CreateClass<T>() where T : new()
            {
                return new T();
            }

            /// <summary>
            /// Упаковывает поле
            /// </summary>
            /// <typeparam name="TContainer"></typeparam>
            /// <typeparam name="TField"></typeparam>
            /// <param name="pma"></param>
            /// <param name="memberInfo"></param>
            /// <returns></returns>
            private static bool PackExtension<TContainer, TField>(PackManagedAutomatic<TContainer> pma, MemberInfo memberInfo)
            {
                var getter = FastGetSetValue.BuildUntypedGetter<TContainer, TField>(memberInfo);
                var setter = FastGetSetValue.BuildUntypedSetter<TContainer, TField>(memberInfo);

                IPackagerMethod<TField> method;

                if (PackManagedAutomaticExtension.CreatingObjectCash.TryGetValue(typeof(TField), out var method_))
                {
                    method = (IPackagerMethod<TField>)method_;
                }
                else
                {
                    method = Packager.GetMethod<TField>();
                }
                bool fixedSize = method.IsFixedSize;

                void PackUp(ref StackWriter writer, TContainer source)
                {
                    method.PackUP(ref writer, getter(source));
                }

                void UnPack(ref StackReader reader, ref TContainer destination)
                {
                    method.UnPack(ref reader, out var field);
                    setter(ref destination, field);
                }

                if (method.IsFixedSize)
                {
                    var meterLocal = new StackMeter();
                    method.GetSize(ref meterLocal, default);
                    int len = meterLocal.GetCalcLength();
                    void GetSize(ref StackMeter meter, TContainer source)
                    {
                        meter.AddFixedSize(len);
                    }
                    pma.MembersGetSize += GetSize;
                }
                else
                {
                    void GetSize(ref StackMeter meter, TContainer source)
                    {
                        method.GetSize(ref meter, getter(source));
                    }
                    pma.MembersGetSize += GetSize;
                }

                pma.MembersPackUp += PackUp;
                pma.MembersUnPack += UnPack;

                return fixedSize;
            }
        }
    }
}