using System;
using System.Reflection;

namespace SmartPackager.Automatic
{
    using ByteStack;
    using PMAE = PackManagedAutomaticExtension_PackArray;

    internal static class PackManagedAutomaticExtension_PackArray
    {
        private static MethodInfo PackArray_MethodInfo => typeof(PMAE).GetMethod(nameof(PMAE.PackArray), MethodUtil.BindingFind);
        private static MethodInfo PackArrayRankOne_MethodInfo => typeof(PMAE).GetMethod(nameof(PMAE.PackArrayRankOne), MethodUtil.BindingFind);


        private delegate void Delegate_PackArray<TArray>(PackManagedAutomatic<TArray> pma);
        private delegate void Delegate_PackArrayRankOne<TElement>(PackManagedAutomatic<TElement[]> pma, IPackagerMethod<TElement> pack);

        public static void Make<TArray>(PackManagedAutomatic<TArray> pma)
        {
            var mi = PackArray_MethodInfo.MakeGenericMethod(typeof(TArray), typeof(TArray).GetElementType());
            var dpa = (Delegate_PackArray<TArray>)mi.CreateDelegate(typeof(Delegate_PackArray<TArray>));
            dpa(pma);
        }



        private static void PackArray<TArray, TElement>(PackManagedAutomatic<TArray> pma)
        {
            IPackagerMethod<TElement> pack = Packager.GetMethods<TElement>();

            if (typeof(TArray).GetArrayRank() == 1)
            {
                PackArrayRankOne((PackManagedAutomatic<TElement[]>)(object)pma, pack);
            }
            else
            {
                throw new NotImplementedException();
            }

        }
        private static void PackArrayRankOne<TElement>(PackManagedAutomatic<TElement[]> pma, IPackagerMethod<TElement> pack)
        {
            void MainPackUp(ref StackWriter writer, TElement[] source, GenericPackUP<TElement[]> packUP)
            {
                if (writer.MakeReference(source))
                {
                    writer.WriteLength(source.Length);
                    packUP(ref writer, source);
                }
            }

            pma.MainPackUp = MainPackUp;

            if (pack.IsFixedSize && typeof(TElement).IsUnManaged())
            {
                void MainUnPack(ref StackReader reader, out TElement[] destination, GenericUnPack<TElement[]> unPack)
                {
                    if (reader.ReadReference())
                    {
                        destination = reader.GetReferenceObject<TElement[]>();
                    }
                    else
                    {
                        int arrLen = reader.ReadLength();
                        destination = null;
                        unPack(ref reader, ref destination);
                        reader.AttachReference(destination);
                    }
                }

                pma.MainUnPack = MainUnPack;
            }
            else
            {
                void MainUnPack(ref StackReader reader, out TElement[] destination, GenericUnPack<TElement[]> unPack)
                {
                    if (reader.ReadReference())
                    {
                        destination = reader.GetReferenceObject<TElement[]>();
                    }
                    else
                    {
                        int arrLen = reader.ReadLength();
                        destination = new TElement[arrLen];
                        reader.AttachReference(destination);
                        unPack(ref reader, ref destination);
                    }
                }

                pma.MainUnPack = MainUnPack;
            }

            void MainGetSize(ref StackMeter meter, TElement[] source, GenericGetSize<TElement[]> getSize)
            {
                if (meter.MakeReference(source))
                {
                    meter.AddLength();
                    getSize(ref meter, source);
                }
            }
            pma.MainGetSize = MainGetSize;

            if (pack.IsFixedSize)
            {
                var meterLocal = new StackMeter();
                pack.GetSize(ref meterLocal, default);
                int oneSize = meterLocal.GetCalcLength();

                void GetSize(ref StackMeter meter, TElement[] source)
                {
                    meter.AddFixedSize(oneSize * source.Length);
                }

                pma.MembersGetSize = GetSize;
            }
            else
            {
                void GetSize(ref StackMeter meter, TElement[] source)
                {
                    for (int i = 0; i < source.Length; i++)
                        pack.GetSize(ref meter, source[i]);
                }

                pma.MembersGetSize = GetSize;
            }

            if (typeof(TElement).IsUnManaged())
            {
                MemoryCopy(pma);
            }
            else
            {
                void MemberPackUP(ref StackWriter writer, TElement[] source)
                {
                    for (int i = 0; i < source.Length; i++)
                        pack.PackUP(ref writer, source[i]);
                }

                void MemberUnPack(ref StackReader reader, ref TElement[] destination)
                {
                    for (int i = 0; i < destination.Length; i++)
                        pack.UnPack(ref reader, out destination[i]);
                }

                pma.MembersPackUp = MemberPackUP;
                pma.MembersUnPack = MemberUnPack;
            }
        }

        #region MemoryCopy
        private static MethodInfo Method_UnsafeMemoryCopy => typeof(PMAE).GetMethod(nameof(PMAE.UnsageMemoryCopy), MethodUtil.BindingFind);
        private delegate void DMemoryCopy<TElement>(PackManagedAutomatic<TElement[]> pma);

        public static void MemoryCopy<TElement>(PackManagedAutomatic<TElement[]> pma)
        {
            Method_UnsafeMemoryCopy.MakeGenericDelegate<DMemoryCopy<TElement>>()(pma);
        }

        private static void MemoryCopyPackUp<TElement>(ref StackWriter writer, TElement[] source) where TElement : unmanaged
        {
            writer.Write(source);
        }


        private static void MemoryCopyUnPack<TElement>(ref StackReader reader, ref TElement[] destination) where TElement : unmanaged
        {
            destination = reader.Read<TElement>(destination.Length);
        }

        private static void UnsageMemoryCopy<TElement>(PackManagedAutomatic<TElement[]> pma) where TElement : unmanaged
        {
            pma.MembersPackUp = MemoryCopyPackUp;
            pma.MembersUnPack = MemoryCopyUnPack;
        }
        #endregion
        /*  
           #region Fixed
           public static void PackUPFixed<TElement>(PackManagedAutomatic<TElement[]> pma)
           {
               MethodInfo mi = UnsafePackUPFixed_MethodInfo.MakeGenericMethod(typeof(TElement));
               return (Delegate_PackUPHeap<TArray>)mi.Invoke(null, new object[] { pack });
           }

           private static void _PackUPFixed<TElement>(PackManagedAutomatic<TElement[]> pma, )
           {

               return (byte* destination, ManagedHeap heap, TElement[] source) =>
               {
                   if (source == null)
                   {
                       *(int*)destination = -1;
                       return sizeof(int);
                   }
                   int length = source.Length;

                   *(int*)destination = length;    //Write Length to data
                   destination += sizeof(int);

                   int size = pack.MainGetSize(default) * length + sizeof(int);

                   for (int i = 0; i < length; i++)
                   {
                       destination += pack.PackUP(destination, source[i]);
                   }

                   return size;
               };
           }
           public static Delegate_UnPackHeap<TArray> UnPackFixed<TArray, TElement>(IPackagerMethod<TElement> pack)
           {
               MethodInfo mi = UnsafeUnPackFixed_MethodInfo.MakeGenericMethod(typeof(TElement));
               return (Delegate_UnPackHeap<TArray>)mi.Invoke(null, new object[] { pack });
           }
           private static unsafe Delegate_UnPackHeap<TElement[]> UnsafeUnPackFixed<TElement>(IPackagerMethod<TElement> pack)
           {
               return (byte* source, ManagedHeap heap, out TElement[] destination) =>
               {
                   int length = *(int*)source;  //Read Length to data
                   if (length < 0)
                   {
                       destination = null;
                       return sizeof(int);
                   }
                   source += sizeof(int);

                   int size = pack.MainGetSize(default) * length + sizeof(int);
                   destination = new TElement[length];
                   heap.AllocateHeap(destination);
                   for (int i = 0; i < length; i++)
                   {
                       source += pack.MainUnPack(source, out destination[i]);
                   }

                   return size;
               };
           }
           #endregion
           */
        /*
        #region MemoryCopy
        public static Delegate_PackUPHeap<TArray> PackUPFixedMemoryCopy<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafePackUPFixedMemoryCopy_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_PackUPHeap<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_PackUPHeap<TElement[]> UnsafePackUPFixedMemoryCopy<TElement>(IPackagerMethod<TElement> pack) where TElement : unmanaged
        {
            return (byte* destination, ManagedHeap heap, TElement[] source) =>
            {
                if (source == null)
                {
                    *(int*)destination = -1;
                    return sizeof(int);
                }
                int length = source.Length;

                *(int*)destination = length;    //Write Length to data
                destination += sizeof(int);

                int size = pack.MainGetSize(default) * length;

                //фикс - если массив нулевой длины 
                if (size > 0)
                    fixed (void* ptr = &source[0])
                        Buffer.MemoryCopy(ptr, destination, size, size);

                size += sizeof(int);
                return size;
            };
        }
        public static Delegate_UnPackHeap<TArray> UnPackFixedMemoryCopy<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafeUnPackFixedMemoryCopy_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_UnPackHeap<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_UnPackHeap<TElement[]> UnsafeUnPackFixedMemoryCopy<TElement>(IPackagerMethod<TElement> pack) where TElement : unmanaged
        {
            return (byte* source, ManagedHeap heap, out TElement[] destination) =>
            {
                int length = *(int*)source;  //Read Length to data
                if (length < 0)
                {
                    destination = null;
                    return sizeof(int);
                }
                source += sizeof(int);

                int size = pack.MainGetSize(default) * length;

                destination = new TElement[length];
                heap.AllocateHeap(destination);
                if (size > 0) // фикс если массив 0 длины
                    fixed (void* ptr = &destination[0])
                        Buffer.MemoryCopy(source, ptr, size, size);
                size += sizeof(int);
                return size;
            };
        }
        #endregion

        #region Dynamic
        public static Delegate_GetSizeHeap<TArray> GetSizeDynamic<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafeGetSizeDynamic_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_GetSizeHeap<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_GetSizeHeap<TElement[]> UnsafeGetSizeDynamic<TElement>(IPackagerMethod<TElement> pack)
        {
            return (TElement[] source, ManagedHeap heap) =>
            {
                if (source == null)
                    return sizeof(int);
                int size = sizeof(int);

                int length = source.Length;
                for (int i = 0; i < length; i++)
                {
                    size += pack.MainGetSize(source[i]);
                }

                return size;
            };
        }
        public static Delegate_PackUPHeap<TArray> PackUPDynamic<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafePackUPDynamic_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_PackUPHeap<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_PackUPHeap<TElement[]> UnsafePackUPDynamic<TElement>(IPackagerMethod<TElement> pack)
        {
            return (byte* destination, ManagedHeap heap, TElement[] source) =>
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
        public static Delegate_UnPackHeap<TArray> UnPackDynamic<TArray, TElement>(IPackagerMethod<TElement> pack)
        {
            MethodInfo mi = UnsafeUnPackDynamic_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_UnPackHeap<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_UnPackHeap<TElement[]> UnsafeUnPackDynamic<TElement>(IPackagerMethod<TElement> pack)
        {
            return (byte* source, ManagedHeap heap, out TElement[] destination) =>
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
                heap.AllocateHeap(destination);

                for (int i = 0; i < length; i++)
                {
                    tmSize = pack.MainUnPack(source, out destination[i]);
                    source += tmSize;
                    size += tmSize;
                }

                return size;
            };
        }
        #endregion

        #region DynamicHeap
        public static Delegate_GetSizeHeap<TArray> GetSizeDynamicHeap<TArray, TElement>(PackStructManagedAutomaticHeap<TElement> pack)
        {
            MethodInfo mi = UnsafeGetSizeDynamicHeap_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_GetSizeHeap<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_GetSizeHeap<TElement[]> UnsafeGetSizeDynamicHeap<TElement>(PackStructManagedAutomaticHeap<TElement> pack)
        {
            return (TElement[] source, ManagedHeap heap) =>
            {
                if (source == null)
                    return sizeof(int);
                int size = sizeof(int);

                int length = source.Length;
                for (int i = 0; i < length; i++)
                {
                    size += pack.MainGetSize(source[i], heap);
                }

                return size;
            };
        }
        public static Delegate_PackUPHeap<TArray> PackUPDynamicHeap<TArray, TElement>(PackStructManagedAutomaticHeap<TElement> pack)
        {
            MethodInfo mi = UnsafePackUPDynamicHeap_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_PackUPHeap<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_PackUPHeap<TElement[]> UnsafePackUPDynamicHeap<TElement>(PackStructManagedAutomaticHeap<TElement> pack)
        {
            return (byte* destination, ManagedHeap heap, TElement[] source) =>
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
                    tmSize = pack.PackUP(destination, heap, source[i]);
                    destination += tmSize;
                    size += tmSize;
                }

                return size;
            };
        }
        public static Delegate_UnPackHeap<TArray> UnPackDynamicHeap<TArray, TElement>(PackStructManagedAutomaticHeap<TElement> pack)
        {
            MethodInfo mi = UnsafeUnPackDynamicHeap_MethodInfo.MakeGenericMethod(typeof(TElement));
            return (Delegate_UnPackHeap<TArray>)mi.Invoke(null, new object[] { pack });
        }
        private static unsafe Delegate_UnPackHeap<TElement[]> UnsafeUnPackDynamicHeap<TElement>(PackStructManagedAutomaticHeap<TElement> pack)
        {
            return (byte* source, ManagedHeap heap, out TElement[] destination) =>
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
                heap.AllocateHeap(destination);

                for (int i = 0; i < length; i++)
                {
                    tmSize = pack.MainUnPack(source, heap, out destination[i]);
                    source += tmSize;
                    size += tmSize;
                }

                return size;
            };
        }
        #endregion
        */

    }
}