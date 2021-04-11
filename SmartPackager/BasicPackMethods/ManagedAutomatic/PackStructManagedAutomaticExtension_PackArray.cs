using System;
using System.Reflection;
using System.Runtime.InteropServices;
using PSMAE = SmartPackager.BasicPackMethods.PackStructManagedAutomaticExtension;

namespace SmartPackager.BasicPackMethods
{
    public static partial class PackStructManagedAutomaticExtension
    {
        private static MethodInfo UnsafeGetSizeFixed_MethodInfo => typeof(PSMAE).GetMethod("UnsafeGetSizeFixed", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo UnsafePackUPFixed_MethodInfo => typeof(PSMAE).GetMethod("UnsafePackUPFixed", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo UnsafeUnPackFixed_MethodInfo => typeof(PSMAE).GetMethod("UnsafeUnPackFixed", BindingFlags.NonPublic | BindingFlags.Static);

        private static MethodInfo UnsafePackUPFixedMemoryCopy_MethodInfo => typeof(PSMAE).GetMethod("UnsafePackUPFixedMemoryCopy", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo UnsafeUnPackFixedMemoryCopy_MethodInfo => typeof(PSMAE).GetMethod("UnsafeUnPackFixedMemoryCopy", BindingFlags.NonPublic | BindingFlags.Static);


        private static MethodInfo UnsafeGetSizeDynamic_MethodInfo => typeof(PSMAE).GetMethod("UnsafeGetSizeDynamic", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo UnsafePackUPDynamic_MethodInfo => typeof(PSMAE).GetMethod("UnsafePackUPDynamic", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo UnsafeUnPackDynamic_MethodInfo => typeof(PSMAE).GetMethod("UnsafeUnPackDynamic", BindingFlags.NonPublic | BindingFlags.Static);


        private static unsafe MethodsData<TArray> PackArray<TArray, TElement>()
        {
            MethodsData<TArray> md;

            IPackagerMethod<TElement> pack = (IPackagerMethod<TElement>)Pack.GetMethods<TElement>();

            if (typeof(TArray).GetArrayRank() == 1)
            {
                md = PackArrayRankOne<TArray, TElement>(pack);
            }
            else
            {
                throw new NotImplementedException();
            }

            return md;
        }
        private static unsafe MethodsData<TArray> PackArrayRankOne<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodsData<TArray> md = new MethodsData<TArray>();

            md.isFixedSize = false;

            if (pack.IsFixedSize)
            {
                md.action_GetSize = GetSizeFixed<TArray, TElement>(pack);

                if (typeof(TElement).IsUnManaged())
                {
                    md.action_PackUP = PackUPFixedMemoryCopy<TArray, TElement>(pack);
                    md.action_UnPack = UnPackFixedMemoryCopy<TArray, TElement>(pack);
                }
                else
                {
                    md.action_PackUP = PackUPFixed<TArray, TElement>(pack);
                    md.action_UnPack = UnPackFixed<TArray, TElement>(pack);
                }
            }
            else
            {
                md.action_GetSize = GetSizeDynamic<TArray, TElement>(pack);
                md.action_PackUP = PackUPDynamic<TArray, TElement>(pack);
                md.action_UnPack = UnPackDynamic<TArray, TElement>(pack);
            }

            return md;
        }


        #region Fixed
        private static unsafe Delegate_GetSize<TArray> GetSizeFixed<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafeGetSizeFixed_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_GetSize<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_GetSize<TElement[]> UnsafeGetSizeFixed<TElement>(IPackagerMethod<TElement> pack)
        {
            long elementSize = pack.GetSize(default);

            return (TElement[] source) =>
            {
                return source.Length * elementSize + sizeof(int);
            };
        }
        private static unsafe Delegate_PackUP<TArray> PackUPFixed<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafePackUPFixed_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_PackUP<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_PackUP<TElement[]> UnsafePackUPFixed<TElement>(IPackagerMethod<TElement> pack)
        {
            return (byte* destination, TElement[] source) =>
            {
                int length = source.Length;

                *(int*)destination = length;    //Write Length to data
                destination += sizeof(int);

                long size = pack.GetSize(default) * length + sizeof(int);

                for (int i = 0; i < length; i++)
                {
                    destination += pack.PackUP(destination, source[i]);
                }

                return size;
            };
        }
        private static unsafe Delegate_UnPack<TArray> UnPackFixed<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafeUnPackFixed_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_UnPack<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_UnPack<TElement[]> UnsafeUnPackFixed<TElement>(IPackagerMethod<TElement> pack)
        {
            return (byte* source, out TElement[] destination) =>
            {
                int length = *(int*)source;  //Read Length to data
                source += sizeof(int);

                long size = pack.GetSize(default) * length + sizeof(int);

                destination = new TElement[length];

                for (int i = 0; i < length; i++)
                {
                    source += pack.UnPack(source, out destination[i]);
                }

                return size;
            };
        }
        #endregion

        #region MemoryCopy
        private static unsafe Delegate_PackUP<TArray> PackUPFixedMemoryCopy<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafePackUPFixedMemoryCopy_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_PackUP<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_PackUP<TElement[]> UnsafePackUPFixedMemoryCopy<TElement>(IPackagerMethod<TElement> pack) where TElement : unmanaged
        {
            return (byte* destination, TElement[] source) =>
            {
                int length = source.Length;

                *(int*)destination = length;    //Write Length to data
                destination += sizeof(int);

                long size = pack.GetSize(default) * length;

                fixed(void* ptr = &source[0])
                    Buffer.MemoryCopy(ptr, destination, size, size);

                size += sizeof(int);
                return size;
            };
        }
        private static unsafe Delegate_UnPack<TArray> UnPackFixedMemoryCopy<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafeUnPackFixedMemoryCopy_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_UnPack<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_UnPack<TElement[]> UnsafeUnPackFixedMemoryCopy<TElement>(IPackagerMethod<TElement> pack) where TElement : unmanaged
        {
            return (byte* source, out TElement[] destination) =>
            {
                int length = *(int*)source;  //Read Length to data
                source += sizeof(int);

                long size = pack.GetSize(default) * length;

                destination = new TElement[length];

                fixed (void* ptr = &destination[0])
                    Buffer.MemoryCopy(source, ptr, size, size);

                size += sizeof(int);
                return size;
            };
        }
        #endregion

        #region Dynamic
        private static unsafe Delegate_GetSize<TArray> GetSizeDynamic<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafeGetSizeDynamic_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_GetSize<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_GetSize<TElement[]> UnsafeGetSizeDynamic<TElement>(IPackagerMethod<TElement> pack)
        {
            return (TElement[] source) =>
            {
                long size = sizeof(int);

                int length = source.Length;
                for (int i = 0; i < length; i++)
                {
                    size += pack.GetSize(source[i]);
                }

                return size;
            };
        }
        private static unsafe Delegate_PackUP<TArray> PackUPDynamic<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafePackUPDynamic_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_PackUP<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_PackUP<TElement[]> UnsafePackUPDynamic<TElement>(IPackagerMethod<TElement> pack)
        {
            return (byte* destination, TElement[] source) =>
            {
                int length = source.Length;

                *(int*)destination = length;    //Write Length to data
                destination += sizeof(int);

                long size = sizeof(int);
                long tmSize;

                for (int i = 0; i < length; i++)
                {
                    tmSize = pack.PackUP(destination, source[i]);
                    destination += tmSize;
                    size += tmSize;
                }

                return size;
            };
        }
        private static unsafe Delegate_UnPack<TArray> UnPackDynamic<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafeUnPackDynamic_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_UnPack<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_UnPack<TElement[]> UnsafeUnPackDynamic<TElement>(IPackagerMethod<TElement> pack)
        {
            return (byte* source, out TElement[] destination) =>
            {
                int length = *(int*)source;  //Read Length to data
                source += sizeof(int);

                long size = sizeof(int);
                long tmSize;

                destination = new TElement[length];

                for (int i = 0; i < length; i++)
                {
                    tmSize = pack.UnPack(source, out destination[i]);
                    source += tmSize;
                    size += tmSize;
                }

                return size;
            };
        }
        #endregion
    }
}
