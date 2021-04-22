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

        private static MethodsData<TContainer> PackContainer<TContainer>()
        {
            //throw new NotImplementedException();

            MethodsData<TContainer> md = new MethodsData<TContainer>();
            SearchPrivateFieldsAttribute osfAttribute = typeof(TContainer).GetCustomAttribute<SearchPrivateFieldsAttribute>();
            BindingFlags bindingFlags = osfAttribute == null ? bindingFlagsDefault : bindingFlagsAll;

            FieldInfo[] fields = (
                from field in typeof(TContainer).GetFields(bindingFlags)
                where !field.IsNotSerialized && !field.IsLiteral && !field.IsInitOnly && !field.IsStatic
                orderby field.Name ascending
                select field
                ).ToArray();

            md.isFixedSize = true;
            Container.Pack(fields, ref md);

            return md;
        }

        // size + sizeof(byte); - null flag
        private static class Container
        {
            private static readonly Dictionary<Type, MethodInfo> CashPackExtension_MethodInfo = new Dictionary<Type, MethodInfo>();
            private static MethodInfo PackExtension_MethodInfo => typeof(Container).GetMethod("PackExtension", BindingFlags.NonPublic | BindingFlags.Static);
            private static MethodInfo CreateClass_MethodInfo => typeof(Container).GetMethod("CreateClass", BindingFlags.NonPublic | BindingFlags.Static);

            private unsafe delegate void PackUp(ref byte* dest, object target, ref long size);
            private unsafe delegate void UnPack(ref byte* sour, object target, ref long size);
            private unsafe delegate void GetSize(object target, ref long size);
            private delegate T Delegate_CreateClass<T>();
            private delegate void Delegate_PackExtension(out PackUp packUP, out UnPack unPack, out GetSize getSize, FieldInfo fi);

            public unsafe static void Pack<TContainer>(FieldInfo[] fields, ref MethodsData<TContainer> data)
            {
                PackUp packUp = null;
                UnPack unPack = null;
                GetSize getSize = null;

                for (int i = 0; i < fields.Length; i++)
                {
                    if (!CashPackExtension_MethodInfo.TryGetValue(fields[i].FieldType, out MethodInfo mi))
                    {
                        mi = PackExtension_MethodInfo.MakeGenericMethod(fields[i].FieldType);
                        CashPackExtension_MethodInfo.Add(fields[i].FieldType, mi);
                    }

                    ((Delegate_PackExtension)mi.CreateDelegate(typeof(Delegate_PackExtension))).Invoke(out PackUp up, out UnPack down, out GetSize gs, fields[i]);

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

                    return size = 1;
                };

                if (typeof(TContainer).IsValueType)
                {
                    data.action_UnPack = (byte* source, out TContainer destination) =>
                    {
                        long size = sizeof(byte);

                        switch (*(ContainerStatus*)source)
                        {
                            case ContainerStatus.empty:
                                destination = default;
                                break;
                            case ContainerStatus.contains:
                                source += 1;
                                destination = default;
                                unPack.Invoke(ref source, destination, ref size);
                                break;
                            default:
                                throw new Exception("Invalid expression for unpacking");
                        }

                        return size;
                    };
                }
                else
                {
                    MethodInfo mi = CreateClass_MethodInfo.MakeGenericMethod(typeof(TContainer));
                    Delegate_CreateClass<TContainer> createClass = (Delegate_CreateClass<TContainer>)mi.CreateDelegate(typeof(Delegate_CreateClass<TContainer>));

                    data.action_UnPack = (byte* source, out TContainer destination) =>
                    {
                        long size = sizeof(byte);

                        switch (*(ContainerStatus*)source)
                        {
                            case ContainerStatus.empty:
                                destination = default;
                                break;
                            case ContainerStatus.contains:
                                source += 1;
                                destination = createClass.Invoke();
                                unPack.Invoke(ref source, destination, ref size);
                                break;
                            default:
                                throw new Exception("Invalid expression for unpacking");
                        }

                        return size;
                    };
                }

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

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Удалите неиспользуемые закрытые члены", Justification = "Reflection.Invoke")]
            private static T CreateClass<T>() where T : new()
            {
                return new T();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Удалите неиспользуемые закрытые члены", Justification = "Reflection.Invoke")]
            private unsafe static void PackExtension<T>(out PackUp packUP, out UnPack unPack, out GetSize getSize, FieldInfo fi)
            {
                IPackagerMethod<T> ipm = Packager.GetMethods<T>();

                packUP = (ref byte* destination, object target, ref long size) =>
                {
                    long s = ipm.PackUP(destination, (T)fi.GetValue(target));
                    size += s;
                    destination += s;
                };

                unPack = (ref byte* sourse, object target, ref long size) =>
                {
                    long s = ipm.UnPack(sourse, out T targ);
                    size += s;
                    sourse += s;
                    fi.SetValue(target, targ);
                };

                getSize = (object target, ref long size) =>
                {
                    size += ipm.GetSize((T)fi.GetValue(target));
                };
            }
        }
    }
}