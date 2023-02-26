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
            IPackagerMethod<TElement> pack = Packager.GetMethod<TElement>();

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
                        destination = null;
                        reader.AttachReference(destination);
                        int arrLen = reader.ReadLength();
                        destination = new TElement[arrLen];
                        unPack(ref reader, ref destination);
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
                        destination = null;
                        reader.AttachReference(destination);
                        int arrLen = reader.ReadLength();
                        destination = new TElement[arrLen];
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
        
    }
}