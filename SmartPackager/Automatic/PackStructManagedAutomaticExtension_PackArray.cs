using System;
using System.Reflection;

namespace SmartPackager.Automatic
{
    internal static partial class PackStructManagedAutomaticExtension
    {
        private static MethodInfo UnsafeGetSizeFixed_MethodInfo => typeof(Array).GetMethod("UnsafeGetSizeFixed", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo UnsafePackUPFixed_MethodInfo => typeof(Array).GetMethod("UnsafePackUPFixed", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo UnsafeUnPackFixed_MethodInfo => typeof(Array).GetMethod("UnsafeUnPackFixed", BindingFlags.NonPublic | BindingFlags.Static);

        private static MethodInfo UnsafePackUPFixedMemoryCopy_MethodInfo => typeof(Array).GetMethod("UnsafePackUPFixedMemoryCopy", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo UnsafeUnPackFixedMemoryCopy_MethodInfo => typeof(Array).GetMethod("UnsafeUnPackFixedMemoryCopy", BindingFlags.NonPublic | BindingFlags.Static);


        private static MethodInfo UnsafeGetSizeDynamic_MethodInfo => typeof(Array).GetMethod("UnsafeGetSizeDynamic", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo UnsafePackUPDynamic_MethodInfo => typeof(Array).GetMethod("UnsafePackUPDynamic", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo UnsafeUnPackDynamic_MethodInfo => typeof(Array).GetMethod("UnsafeUnPackDynamic", BindingFlags.NonPublic | BindingFlags.Static);

        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Удалите неиспользуемые закрытые члены", Justification = "Reflection.Invoke")]
        private static MethodsData<TArray> PackArray<TArray, TElement>()
        {
            MethodsData<TArray> md;

            IPackagerMethod<TElement> pack = Packager.GetMethods<TElement>();

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
        private static MethodsData<TArray> PackArrayRankOne<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodsData<TArray> md = new MethodsData<TArray>
            {
                IsFixedSize = false
            };

            if (pack.IsFixedSize)
            {
                md.Action_GetSize = Array.GetSizeFixed<TArray, TElement>(pack);

                if (typeof(TElement).IsUnManaged())
                {
                    md.Action_PackUP = Array.PackUPFixedMemoryCopy<TArray, TElement>(pack);
                    md.Action_UnPack = Array.UnPackFixedMemoryCopy<TArray, TElement>(pack);
                }
                else
                {
                    md.Action_PackUP = Array.PackUPFixed<TArray, TElement>(pack);
                    md.Action_UnPack = Array.UnPackFixed<TArray, TElement>(pack);
                }
            }
            else
            {
                md.Action_GetSize = Array.GetSizeDynamic<TArray, TElement>(pack);
                md.Action_PackUP = Array.PackUPDynamic<TArray, TElement>(pack);
                md.Action_UnPack = Array.UnPackDynamic<TArray, TElement>(pack);
            }

            return md;
        }

        private static class Array
        {
            #region Fixed
            public static Delegate_GetSize<TArray> GetSizeFixed<TArray, TElement>(IPackagerMethod<TElement> pack)
            {
                MethodInfo mi = UnsafeGetSizeFixed_MethodInfo.MakeGenericMethod(typeof(TElement));
                return (Delegate_GetSize<TArray>)mi.Invoke(null, new object[] { pack });
            }
            private static unsafe Delegate_GetSize<TElement[]> UnsafeGetSizeFixed<TElement>(IPackagerMethod<TElement> pack)
            {
                int elementSize = pack.GetSize(default);

                return (TElement[] source) =>
                {
                    if (source == null) return sizeof(int);
                    return source.Length * elementSize + sizeof(int);
                };
            }
            public static Delegate_PackUP<TArray> PackUPFixed<TArray, TElement>(IPackagerMethod<TElement> pack)
            {
                MethodInfo mi = UnsafePackUPFixed_MethodInfo.MakeGenericMethod(typeof(TElement));
                return (Delegate_PackUP<TArray>)mi.Invoke(null, new object[] { pack });
            }
            private static unsafe Delegate_PackUP<TElement[]> UnsafePackUPFixed<TElement>(IPackagerMethod<TElement> pack)
            {
                return (byte* destination, TElement[] source) =>
                {
                    if (source == null)
                    {
                        *(int*)destination = -1;
                        return sizeof(int);
                    }
                    int length = source.Length;

                    *(int*)destination = length;    //Write Length to data
                    destination += sizeof(int);

                    int size = pack.GetSize(default) * length + sizeof(int);

                    for (int i = 0; i < length; i++)
                    {
                        destination += pack.PackUP(destination, source[i]);
                    }

                    return size;
                };
            }
            public static Delegate_UnPack<TArray> UnPackFixed<TArray, TElement>(IPackagerMethod<TElement> pack)
            {
                MethodInfo mi = UnsafeUnPackFixed_MethodInfo.MakeGenericMethod(typeof(TElement));
                return (Delegate_UnPack<TArray>)mi.Invoke(null, new object[] { pack });
            }
            private static unsafe Delegate_UnPack<TElement[]> UnsafeUnPackFixed<TElement>(IPackagerMethod<TElement> pack)
            {
                return (byte* source, out TElement[] destination) =>
                {
                    int length = *(int*)source;  //Read Length to data
                    if (length < 0)
                    {
                        destination = null;
                        return sizeof(int);
                    }
                    source += sizeof(int);

                    int size = pack.GetSize(default) * length + sizeof(int);
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
            public static Delegate_PackUP<TArray> PackUPFixedMemoryCopy<TArray, TElement>(IPackagerMethod<TElement> pack)
            {
                MethodInfo mi = UnsafePackUPFixedMemoryCopy_MethodInfo.MakeGenericMethod(typeof(TElement));
                return (Delegate_PackUP<TArray>)mi.Invoke(null, new object[] { pack });
            }
            private static unsafe Delegate_PackUP<TElement[]> UnsafePackUPFixedMemoryCopy<TElement>(IPackagerMethod<TElement> pack) where TElement : unmanaged
            {
                return (byte* destination, TElement[] source) =>
                {
                    if (source == null) 
                    {
                        *(int*)destination = -1;
                        return sizeof(int);
                    }
                    int length = source.Length;

                    *(int*)destination = length;    //Write Length to data
                    destination += sizeof(int);

                    int size = pack.GetSize(default) * length;

                    //фикс - если массив нулевой длины 
                    if(size>0)
                    fixed (void* ptr = &source[0])
                        Buffer.MemoryCopy(ptr, destination, size, size);

                    size += sizeof(int);
                    return size;
                };
            }
            public static Delegate_UnPack<TArray> UnPackFixedMemoryCopy<TArray, TElement>(IPackagerMethod<TElement> pack)
            {
                MethodInfo mi = UnsafeUnPackFixedMemoryCopy_MethodInfo.MakeGenericMethod(typeof(TElement));
                return (Delegate_UnPack<TArray>)mi.Invoke(null, new object[] { pack });
            }
            private static unsafe Delegate_UnPack<TElement[]> UnsafeUnPackFixedMemoryCopy<TElement>(IPackagerMethod<TElement> pack) where TElement : unmanaged
            {
                return (byte* source, out TElement[] destination) =>
                {
                    int length = *(int*)source;  //Read Length to data
                    if (length < 0)
                    {
                        destination = null;
                        return sizeof(int);
                    }
                    source += sizeof(int);

                    int size = pack.GetSize(default) * length;

                    destination = new TElement[length];
                    if (size > 0) // фикс если массив 0 длины
                        fixed (void* ptr = &destination[0])
                            Buffer.MemoryCopy(source, ptr, size, size);
                    size += sizeof(int);
                    return size;
                };
            }
            #endregion

            #region Dynamic
            public static Delegate_GetSize<TArray> GetSizeDynamic<TArray, TElement>(IPackagerMethod<TElement> pack)
            {
                MethodInfo mi = UnsafeGetSizeDynamic_MethodInfo.MakeGenericMethod(typeof(TElement));
                return (Delegate_GetSize<TArray>)mi.Invoke(null, new object[] { pack });
            }
            private static unsafe Delegate_GetSize<TElement[]> UnsafeGetSizeDynamic<TElement>(IPackagerMethod<TElement> pack)
            {
                return (TElement[] source) =>
                {
                    if (source == null)
                        return sizeof(int);
                    int size = sizeof(int);

                    int length = source.Length;
                    for (int i = 0; i < length; i++)
                    {
                        size += pack.GetSize(source[i]);
                    }

                    return size;
                };
            }
            public static Delegate_PackUP<TArray> PackUPDynamic<TArray, TElement>(IPackagerMethod<TElement> pack)
            {
                MethodInfo mi = UnsafePackUPDynamic_MethodInfo.MakeGenericMethod(typeof(TElement));
                return (Delegate_PackUP<TArray>)mi.Invoke(null, new object[] { pack });
            }
            private static unsafe Delegate_PackUP<TElement[]> UnsafePackUPDynamic<TElement>(IPackagerMethod<TElement> pack)
            {
                return (byte* destination, TElement[] source) =>
                {
                    if (source == null)
                    {
                        *(int*)destination = -1;
                        return sizeof(int);
                    }
                    int length = source.Length;

                    *(int*)destination = length;    //Write Length to data
                    destination += sizeof(int);

                    int size = sizeof(int);
                    int tmSize;

                    for (int i = 0; i < length; i++)
                    {
                        tmSize = pack.PackUP(destination, source[i]);
                        destination += tmSize;
                        size += tmSize;
                    }

                    return size;
                };
            }
            public static Delegate_UnPack<TArray> UnPackDynamic<TArray, TElement>(IPackagerMethod<TElement> pack)
            {
                MethodInfo mi = UnsafeUnPackDynamic_MethodInfo.MakeGenericMethod(typeof(TElement));
                return (Delegate_UnPack<TArray>)mi.Invoke(null, new object[] { pack });
            }
            private static unsafe Delegate_UnPack<TElement[]> UnsafeUnPackDynamic<TElement>(IPackagerMethod<TElement> pack)
            {
                return (byte* source, out TElement[] destination) =>
                {
                    int length = *(int*)source;  //Read Length to data
                    if (length < 0)
                    {
                        destination = null;
                        return sizeof(int);
                    }
                    source += sizeof(int);

                    int size = sizeof(int);
                    int tmSize;

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
}