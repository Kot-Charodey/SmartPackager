using System;
using System.Linq;
using System.Reflection;

namespace SmartPackager.Automatic
{
    internal static partial class PackStructManagedAutomaticExtension
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

        private static MethodsDataHeap<TContainer> PackContainer<TContainer>()
        {
            //throw new NotImplementedException();

            MethodsDataHeap<TContainer> md = new MethodsDataHeap<TContainer>();
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

            Container.Pack(memberInfo, ref md);

            return md;
        }

        // size + sizeof(byte); - null flag
        private static class Container
        {
            private static MethodInfo PackExtension_MethodInfo => typeof(Container).GetMethod("PackExtension", BindingFlags.NonPublic | BindingFlags.Static);
            private static MethodInfo CreateClass_MethodInfo => typeof(Container).GetMethod("CreateClass", BindingFlags.NonPublic | BindingFlags.Static);

            private unsafe delegate void PackUp<TContainer>(ref byte* dest, ManagedHeap heap, in TContainer target, ref int size);
            private unsafe delegate void UnPack<TContainer>(ref byte* sour, ManagedHeap heap, ref TContainer target, ref int size);
            private unsafe delegate void GetSize<TContainer>(TContainer target, ManagedHeap heap, ref int size);
            private delegate T Delegate_CreateClass<T>();
            private delegate bool Delegate_PackExtension<TContainer>(out PackUp<TContainer> packUP, out UnPack<TContainer> unPack, out GetSize<TContainer> getSize, MemberInfo memberInfo);


            public unsafe static void Pack<TContainer>(MemberInfo[] membersInfo, ref MethodsDataHeap<TContainer> data)
            {
                PackUp<TContainer> packUp = null;
                UnPack<TContainer> unPack = null;
                GetSize<TContainer> getSize = null;

                data.IsFixedSize = true;

                for (int i = 0; i < membersInfo.Length; i++)
                {
                    var typeTContainer = typeof(TContainer);
                    var memberInfo = membersInfo[i];
                    var typeTField = memberInfo.GetUnderlyingType();

                    MethodInfo mi = PackExtension_MethodInfo.MakeGenericMethod(typeTContainer, typeTField);


                    data.IsFixedSize &= ((Delegate_PackExtension<TContainer>)mi.CreateDelegate(typeof(Delegate_PackExtension<TContainer>))).
                        Invoke(out PackUp<TContainer> up, out UnPack<TContainer> down, out GetSize<TContainer> gs, memberInfo);

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
                    data.Action_PackUP = (byte* destination, ManagedHeap heap, TContainer source) =>
                    {
                        int size = 0;
                        packUp.Invoke(ref destination, heap, source, ref size);

                        return size;
                    };

                    data.Action_UnPack = (byte* source, ManagedHeap heap, out TContainer destination) =>
                    {
                        int size = 0;
                        destination = default;
                        unPack.Invoke(ref source, heap, ref destination, ref size);

                        return size;
                    };

                    if (data.IsFixedSize)
                    {
                        int cashSize = 0;
                        getSize.Invoke(default, new ManagedHeap(), ref cashSize);
                        data.Action_GetSize = (TContainer source, ManagedHeap heap) =>
                        {
                            return cashSize;
                        };
                    }
                    else
                    {
                        data.Action_GetSize = (TContainer source, ManagedHeap heap) =>
                        {
                            int size = 0;
                            getSize.Invoke(source, heap, ref size);

                            return size;
                        };
                    }
                }
                else //if class
                {
                    data.Action_PackUP = (byte* destination, ManagedHeap heap, TContainer source) =>
                    {
                        int size = 0;
                        heap.AllocateHeap(source, destination - sizeof(byte));
                        packUp.Invoke(ref destination, heap, source, ref size);

                        return size;
                    };

                    MethodInfo mi = CreateClass_MethodInfo.MakeGenericMethod(typeof(TContainer));
                    Delegate_CreateClass<TContainer> createClass = (Delegate_CreateClass<TContainer>)mi.CreateDelegate(typeof(Delegate_CreateClass<TContainer>));

                    data.Action_UnPack = (byte* source, ManagedHeap heap, out TContainer destination) =>
                    {
                        int size = 0;

                        destination = createClass.Invoke();
                        heap.AllocateHeap(destination, source - sizeof(byte));
                        unPack.Invoke(ref source, heap, ref destination, ref size);

                        return size;
                    };

                    if (data.IsFixedSize)
                    {
                        int cashSize = 0;
                        TContainer container = createClass.Invoke();
                        getSize.Invoke(container, new ManagedHeap(container), ref cashSize);
                        data.Action_GetSize = (TContainer source, ManagedHeap heap) =>
                        {
                            return cashSize;
                        };
                    }
                    else
                    {
                        data.Action_GetSize = (TContainer source, ManagedHeap heap) =>
                        {
                            int size = 0;
                            heap.AllocateHeap(source);
                            getSize.Invoke(source, heap, ref size);
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
                var getter = FastGetSetValue.BuildUntypedGetter<TContainer, TField>(memberInfo);
                var setter = FastGetSetValue.BuildUntypedSetter<TContainer, TField>(memberInfo);

                IPackagerMethod<TField> ipm=null;
                object ipm_ref=null;
                bool isHeap = false;
                if(CreatingPackagerCash.TryGetValue(typeof(TField),out var method))
                {
                    ipm_ref = method;
                    isHeap = true;
                }
                else
                {
                    ipm = Packager.GetMethods<TField>();
                    isHeap = ipm is PackStructManagedAutomaticHeap<TField> pp;
                    if (isHeap)
                        ipm_ref = new RefObject<PackStructManagedAutomaticHeap<TField>>((PackStructManagedAutomaticHeap<TField>)ipm);
                }
                

                if (isHeap)
                {
                    var ipm_heapRef = (RefObject<PackStructManagedAutomaticHeap<TField>>)ipm_ref;

                    packUP = (ref byte* destination, ManagedHeap heap, in TContainer target, ref int size) =>
                    {
                        int s = ipm_heapRef.Object.PackUP(destination, heap, getter(target));
                        size += s;
                        destination += s;
                    };

                    unPack = (ref byte* sourse, ManagedHeap heap, ref TContainer target, ref int size) =>
                    {
                        int s = ipm_heapRef.Object.UnPack(sourse, heap, out TField targ);
                        size += s;
                        sourse += s;
                        setter(ref target, targ);
                    };

                    getSize = (TContainer target, ManagedHeap heap, ref int size) =>
                    {
                        size += ipm_heapRef.Object.GetSize(getter(target), heap);
                    };

                    return false;
                }
                else
                {
                    packUP = (ref byte* destination, ManagedHeap heap, in TContainer target, ref int size) =>
                    {
                        int s = ipm.PackUP(destination, getter(target));
                        size += s;
                        destination += s;
                    };

                    unPack = (ref byte* sourse, ManagedHeap heap, ref TContainer target, ref int size) =>
                    {
                        int s = ipm.UnPack(sourse, out TField targ);
                        size += s;
                        sourse += s;
                        setter(ref target, targ);
                    };

                    getSize = (TContainer target, ManagedHeap heap, ref int size) =>
                    {
                        size += ipm.GetSize(getter(target));
                    };
                    return true;
                }
            }
        }
    }
}