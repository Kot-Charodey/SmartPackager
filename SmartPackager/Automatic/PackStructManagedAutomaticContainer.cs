using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SmartPackager.Automatic
{
    internal static partial class PackStructManagedAutomaticExtension
    {
        private const BindingFlags bindingFlagsDefault = BindingFlags.Public | BindingFlags.Instance;
        private const BindingFlags bindingFlagsAll = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;


        private enum ContainerStatus : byte
        {
            error,
            empty,
            contains,
        }

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

        private static MethodsData<TContainer> PackContainer<TContainer>()
        {
            //throw new NotImplementedException();

            MethodsData<TContainer> md = new MethodsData<TContainer>();
            SearchPrivateFieldsAttribute osfAttribute = typeof(TContainer).GetCustomAttribute<SearchPrivateFieldsAttribute>();
            BindingFlags bindingFlags = osfAttribute == null ? bindingFlagsDefault : bindingFlagsAll;

            //ищет свойства и поля

            var fields = 
                from field in typeof(TContainer).GetFields(bindingFlags)
                where !field.IsNotSerialized && !field.IsLiteral && !field.IsInitOnly && !field.IsStatic
                where field.GetCustomAttribute<NotPack>() == null
                where !field.Name.Contains("k__BackingField")
                select (MemberInfo)field;

            var properties =
                from property in typeof(TContainer).GetProperties(bindingFlags)
                where property.CanWrite && property.CanRead
                where (property.GetGetMethod(false)) == null
                where property.GetCustomAttribute<NotPack>() == null
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

            md.isFixedSize = true;
            Container.Pack(memberInfo, ref md);

            return md;
        }

        // size + sizeof(byte); - null flag
        private static class Container
        {
            private static MethodInfo PackExtension_MethodInfo => typeof(Container).GetMethod("PackExtension", BindingFlags.NonPublic | BindingFlags.Static);
            private static MethodInfo CreateClass_MethodInfo => typeof(Container).GetMethod("CreateClass", BindingFlags.NonPublic | BindingFlags.Static);

            private unsafe delegate void PackUp<TContainer>(ref byte* dest, in TContainer target, ref long size);
            private unsafe delegate void UnPack<TContainer>(ref byte* sour, ref TContainer target, ref long size);
            private unsafe delegate void GetSize<TContainer>(TContainer target, ref long size);
            private delegate T Delegate_CreateClass<T>();
            private delegate bool Delegate_PackExtension<TContainer>(out PackUp<TContainer> packUP, out UnPack<TContainer> unPack, out GetSize<TContainer> getSize, MemberInfo memberInfo);

            public unsafe static void Pack<TContainer>(MemberInfo[] memberInfo, ref MethodsData<TContainer> data)
            {
                PackUp<TContainer> packUp = null;
                UnPack<TContainer> unPack = null;
                GetSize<TContainer> getSize = null;

                bool isFixedSize = true;

                for (int i = 0; i < memberInfo.Length; i++)
                {
                    MethodInfo mi = PackExtension_MethodInfo.MakeGenericMethod(typeof(TContainer), memberInfo[i].GetUnderlyingType());

                    isFixedSize &= ((Delegate_PackExtension<TContainer>)mi.CreateDelegate(typeof(Delegate_PackExtension<TContainer>))).
                        Invoke(out PackUp<TContainer> up, out UnPack<TContainer> down, out GetSize<TContainer> gs, memberInfo[i]);

                    if (i == 0)
                    {
                        packUp = up;
                        unPack = down;
                        getSize = gs;
                    }
                    else
                    {
                        packUp += up;
                        unPack += down;
                        getSize += gs;
                    }
                }

                //if structure
                if (typeof(TContainer).IsValueType)
                {
                    data.action_PackUP = (byte* destination, TContainer source) =>
                    {
                        long size = 0;
                        packUp.Invoke(ref destination, source, ref size);

                        return size;
                    };

                    data.action_UnPack = (byte* source, out TContainer destination) =>
                    {
                        long size = 0;
                        destination = default;
                        unPack.Invoke(ref source, ref destination, ref size);

                        return size;
                    };

                    if (isFixedSize)
                    {
                        long clcSize = 0;
                        getSize.Invoke(default, ref clcSize);

                        data.action_GetSize = (TContainer source) =>
                        {
                            return clcSize;
                        };
                    }
                    else
                    {
                        data.action_GetSize = (TContainer source) =>
                        {
                            long size = 0;
                            if (source != null)
                            {
                                getSize.Invoke(source, ref size);
                            }

                            return size;
                        };
                    }
                }
                else //if class
                {
                    data.action_PackUP = (byte* destination, TContainer source) =>
                    {
                        long size = sizeof(byte);

                        if (source == null)
                        {
                            *destination = 1;
                        }
                        else
                        {
                            *destination = 2;
                            destination += 1;
                            packUp.Invoke(ref destination, source, ref size);
                        }

                        return size;
                    };

                    MethodInfo mi = CreateClass_MethodInfo.MakeGenericMethod(typeof(TContainer));
                    Delegate_CreateClass<TContainer> createClass = (Delegate_CreateClass<TContainer>)mi.CreateDelegate(typeof(Delegate_CreateClass<TContainer>));

                    data.action_UnPack = (byte* source, out TContainer destination) =>
                    {
                        long size = sizeof(byte);

                        switch (*(ContainerStatus*)source)
                        {
                            case ContainerStatus.empty:
                                destination = default;//null
                                break;
                            case ContainerStatus.contains:
                                source += 1;
                                destination = createClass.Invoke();
                                unPack.Invoke(ref source, ref destination, ref size);
                                break;
                            default:
                                throw new Exception("Invalid expression for unpacking");
                        }

                        return size;
                    };

                    if (isFixedSize)
                    {
                        long clcSize = sizeof(byte);
                        getSize.Invoke(createClass.Invoke(), ref clcSize);

                        data.action_GetSize = (TContainer source) =>
                        {
                            return clcSize;
                        };
                    }
                    else
                    {
                        data.action_GetSize = (TContainer source) =>
                        {
                            long size = sizeof(byte);
                            if (source != null)
                            {
                                getSize.Invoke(source, ref size);
                            }

                            return size;
                        };
                    }
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Удалите неиспользуемые закрытые члены", Justification = "Reflection.Invoke")]
            private static T CreateClass<T>() where T : new()
            {
                return new T();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Удалите неиспользуемые закрытые члены", Justification = "Reflection.Invoke")]
            private unsafe static bool PackExtension<TContainer, TField>(out PackUp<TContainer> packUP, out UnPack<TContainer> unPack, out GetSize<TContainer> getSize, MemberInfo memberInfo)
            {
                IPackagerMethod<TField> ipm = Packager.GetMethods<TField>();
                var getter = FastGetSetValue.BuildUntypedGetter<TContainer,TField>(memberInfo);
                var setter = FastGetSetValue.BuildUntypedSetter<TContainer,TField>(memberInfo);

                packUP = (ref byte* destination, in TContainer target, ref long size) =>
                {
                    long s = ipm.PackUP(destination, getter(target));
                    size += s;
                    destination += s;
                };

                unPack = (ref byte* sourse, ref TContainer target, ref long size) =>
                {
                    long s = ipm.UnPack(sourse, out TField targ);
                    size += s;
                    sourse += s;
                    setter(target, targ);
                };

                getSize = (TContainer target, ref long size) =>
                {
                    size += ipm.GetSize(getter(target));
                };

                return ipm.IsFixedSize;
            }
        }
    }
}