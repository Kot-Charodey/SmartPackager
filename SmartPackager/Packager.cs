namespace SmartPackager
{
    using ByteStack;
    /// <summary>
    /// Creates a packer for the selected set of types
    /// </summary>
    public static class Packager
    {
        internal static bool SetupDone=false;
        private readonly static object Sync = new object();

        internal static IPackagerMethod<T> GetMethods<T>()
        {
            lock (Sync) //for thread safety
            {
                if (!SetupDone)
                {
                    PackMethods.SetupPackMethods();
                    SetupDone = true;
                }

                //searches for a method implementation for this type or tries to generate
                if (PackMethods.PackMethodsDictionary.TryGetValue(typeof(T), out IPackagerMethodGeneric ipm))
                {
                    return (IPackagerMethod<T>)ipm;
                }
                else if (PackMethods.PackGenericNoCreatedMethodsDictionary.TryGetValue(typeof(T).GetFullName(), out var type))
                {
                    return (IPackagerMethod<T>)Automatic.GenericFactoryExtension.Make<T>(type);
                }
                else if (typeof(T).IsUnManaged())
                {
                    return (IPackagerMethod<T>)Automatic.PackStructUnmanagedAutomaticExtension.Make<T>();
                }
                else
                {
                    return (IPackagerMethod<T>)Automatic.PackStructManagedAutomaticExtension.Make<T>();
                }
            }
        }

        #region Utilites
        /// <summary>
        /// Does this type have a static size
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsFixedType<T>()
        {
            return GetMethods<T>().IsFixedSize;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1> Create<T1>()
        {
            return new M<T1>(GetMethods<T1>());
        }
#if TODO
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2> Create<T1, T2>()
        {
            return new M<T1, T2>(GetMethods<T1>(), GetMethods<T2>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3> Create<T1, T2, T3>()
        {
            return new M<T1, T2, T3>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4> Create<T1, T2, T3, T4>()
        {
            return new M<T1, T2, T3, T4>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>()
        {
            return new M<T1, T2, T3, T4, T5>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>()
        {
            return new M<T1, T2, T3, T4, T5, T6>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6, T7, T8>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>(), GetMethods<T10>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>(), GetMethods<T10>(), GetMethods<T11>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>(), GetMethods<T10>(), GetMethods<T11>(), GetMethods<T12>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>(), GetMethods<T10>(), GetMethods<T11>(), GetMethods<T12>(), GetMethods<T13>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>(), GetMethods<T10>(), GetMethods<T11>(), GetMethods<T12>(), GetMethods<T13>(), GetMethods<T14>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>(), GetMethods<T10>(), GetMethods<T11>(), GetMethods<T12>(), GetMethods<T13>(), GetMethods<T14>(), GetMethods<T15>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>(), GetMethods<T10>(), GetMethods<T11>(), GetMethods<T12>(), GetMethods<T13>(), GetMethods<T14>(), GetMethods<T15>(), GetMethods<T16>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>(), GetMethods<T10>(), GetMethods<T11>(), GetMethods<T12>(), GetMethods<T13>(), GetMethods<T14>(), GetMethods<T15>(), GetMethods<T16>(), GetMethods<T17>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>(), GetMethods<T10>(), GetMethods<T11>(), GetMethods<T12>(), GetMethods<T13>(), GetMethods<T14>(), GetMethods<T15>(), GetMethods<T16>(), GetMethods<T17>(), GetMethods<T18>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>(), GetMethods<T10>(), GetMethods<T11>(), GetMethods<T12>(), GetMethods<T13>(), GetMethods<T14>(), GetMethods<T15>(), GetMethods<T16>(), GetMethods<T17>(), GetMethods<T18>(), GetMethods<T19>());
        }
        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>()
        {
            return new M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(GetMethods<T1>(), GetMethods<T2>(), GetMethods<T3>(), GetMethods<T4>(), GetMethods<T5>(), GetMethods<T6>(), GetMethods<T7>(), GetMethods<T8>(), GetMethods<T9>(), GetMethods<T10>(), GetMethods<T11>(), GetMethods<T12>(), GetMethods<T13>(), GetMethods<T14>(), GetMethods<T15>(), GetMethods<T16>(), GetMethods<T17>(), GetMethods<T18>(), GetMethods<T19>(), GetMethods<T20>());
        }
#endif
#endregion

#region M1
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1>
        {
            private readonly IPackagerMethod<T1> Ipm1;

            internal M(
                IPackagerMethod<T1> ipm1)
            {
                Ipm1 = ipm1;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1)
            {
                ByteMeter meter = new ByteMeter();
                Ipm1.GetSize(ref meter, t1);
                return meter.GetCalcLength();
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public void PackUP(byte[] destination, int offset, T1 t1)
            {
                UnsafeArray.UseArray(destination, offset, destination.Length, (ref UnsafeArray array) =>
                {
                    ByteWriter writer = new ByteWriter(array);
                    Ipm1.PackUP(ref writer, t1);
                });
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public byte[] PackUP(T1 t1)
            {
                byte[] destination = new byte[CalcNeedSize(t1)];
                UnsafeArray.UseArray(destination, 0, destination.Length, (ref UnsafeArray array) =>
                {
                    ByteWriter writer = new ByteWriter(array);
                    Ipm1.PackUP(ref writer, t1);
                });
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1)
            {
                T1 t1_=default;
                UnsafeArray.UseArray(source, offset, source.Length, (ref UnsafeArray array) =>
                {
                    ByteReader reader = new ByteReader(array);
                    Ipm1.UnPack(ref reader, out t1_);
                });
                t1 = t1_;
            }
        }
#endregion
#if TODO
#region M2
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe int PackUP(byte* destination, T1 t1, T2 t2)
            {
                int size = 0;
                int offset;

                offset = Ipm1.PackUP(destination, t1);
                destination += offset;
                size += offset;
                size += Ipm2.PackUP(destination, t2);
                return size;
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    Ipm2.PackUP(point, t2);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    Ipm2.PackUP(point, t2);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe int UnPack(byte* source, out T1 t1, out T2 t2)
            {
                int size = 0;
                int offset;
                offset = Ipm1.UnPack(source, out t1);
                size += offset;
                source += offset;
                size += Ipm2.UnPack(source, out t2);
                return size;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    Ipm2.UnPack(point, out t2);
                }
            }
        }
#endregion
#region M3
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    Ipm3.PackUP(point, t3);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    Ipm3.PackUP(point, t3);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    Ipm3.UnPack(point, out t3);
                }
            }
        }
#endregion
#region M4
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    Ipm4.PackUP(point, t4);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    Ipm4.PackUP(point, t4);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    Ipm4.UnPack(point, out t4);
                }
            }
        }
#endregion
#region M5
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    Ipm5.PackUP(point, t5);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    Ipm5.PackUP(point, t5);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    Ipm5.UnPack(point, out t5);
                }
            }
        }
#endregion
#region M6
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    Ipm6.PackUP(point, t6);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    Ipm6.PackUP(point, t6);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    Ipm6.UnPack(point, out t6);
                }
            }
        }
#endregion
#region M7
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    Ipm7.PackUP(point, t7);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    Ipm7.PackUP(point, t7);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    Ipm7.UnPack(point, out t7);
                }
            }
        }
#endregion
#region M8
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    Ipm8.PackUP(point, t8);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    Ipm8.PackUP(point, t8);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    Ipm8.UnPack(point, out t8);
                }
            }
        }
#endregion
#region M9
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                }
            }
        }
#endregion
#region M10
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;
            private readonly IPackagerMethod<T10> Ipm10;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9,
                IPackagerMethod<T10> ipm10)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
                Ipm10 = ipm10;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9) +
                       Ipm10.GetSize(t10);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    Ipm10.PackUP(point, t10);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    Ipm10.PackUP(point, t10);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9, out T10 t10)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                    Ipm10.UnPack(point, out t10);
                }
            }
        }
#endregion
#region M11
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;
            private readonly IPackagerMethod<T10> Ipm10;
            private readonly IPackagerMethod<T11> Ipm11;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9,
                IPackagerMethod<T10> ipm10,
                IPackagerMethod<T11> ipm11)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
                Ipm10 = ipm10;
                Ipm11 = ipm11;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9) +
                       Ipm10.GetSize(t10) +
                       Ipm11.GetSize(t11);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    Ipm11.PackUP(point, t11);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    Ipm11.PackUP(point, t11);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9, out T10 t10, out T11 t11)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                    point += Ipm10.UnPack(point, out t10);
                    Ipm11.UnPack(point, out t11);
                }
            }
        }
#endregion
#region M12
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;
            private readonly IPackagerMethod<T10> Ipm10;
            private readonly IPackagerMethod<T11> Ipm11;
            private readonly IPackagerMethod<T12> Ipm12;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9,
                IPackagerMethod<T10> ipm10,
                IPackagerMethod<T11> ipm11,
                IPackagerMethod<T12> ipm12)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
                Ipm10 = ipm10;
                Ipm11 = ipm11;
                Ipm12 = ipm12;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9) +
                       Ipm10.GetSize(t10) +
                       Ipm11.GetSize(t11) +
                       Ipm12.GetSize(t12);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    Ipm12.PackUP(point, t12);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    Ipm12.PackUP(point, t12);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9, out T10 t10, out T11 t11, out T12 t12)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                    point += Ipm10.UnPack(point, out t10);
                    point += Ipm11.UnPack(point, out t11);
                    Ipm12.UnPack(point, out t12);
                }
            }
        }
#endregion
#region M13
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;
            private readonly IPackagerMethod<T10> Ipm10;
            private readonly IPackagerMethod<T11> Ipm11;
            private readonly IPackagerMethod<T12> Ipm12;
            private readonly IPackagerMethod<T13> Ipm13;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9,
                IPackagerMethod<T10> ipm10,
                IPackagerMethod<T11> ipm11,
                IPackagerMethod<T12> ipm12,
                IPackagerMethod<T13> ipm13)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
                Ipm10 = ipm10;
                Ipm11 = ipm11;
                Ipm12 = ipm12;
                Ipm13 = ipm13;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9) +
                       Ipm10.GetSize(t10) +
                       Ipm11.GetSize(t11) +
                       Ipm12.GetSize(t12) +
                       Ipm13.GetSize(t13);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    Ipm13.PackUP(point, t13);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    Ipm13.PackUP(point, t13);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9, out T10 t10, out T11 t11, out T12 t12, out T13 t13)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                    point += Ipm10.UnPack(point, out t10);
                    point += Ipm11.UnPack(point, out t11);
                    point += Ipm12.UnPack(point, out t12);
                    Ipm13.UnPack(point, out t13);
                }
            }
        }
#endregion
#region M14
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;
            private readonly IPackagerMethod<T10> Ipm10;
            private readonly IPackagerMethod<T11> Ipm11;
            private readonly IPackagerMethod<T12> Ipm12;
            private readonly IPackagerMethod<T13> Ipm13;
            private readonly IPackagerMethod<T14> Ipm14;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9,
                IPackagerMethod<T10> ipm10,
                IPackagerMethod<T11> ipm11,
                IPackagerMethod<T12> ipm12,
                IPackagerMethod<T13> ipm13,
                IPackagerMethod<T14> ipm14)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
                Ipm10 = ipm10;
                Ipm11 = ipm11;
                Ipm12 = ipm12;
                Ipm13 = ipm13;
                Ipm14 = ipm14;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9) +
                       Ipm10.GetSize(t10) +
                       Ipm11.GetSize(t11) +
                       Ipm12.GetSize(t12) +
                       Ipm13.GetSize(t13) +
                       Ipm14.GetSize(t14);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    Ipm14.PackUP(point, t14);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    Ipm14.PackUP(point, t14);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9, out T10 t10, out T11 t11, out T12 t12, out T13 t13, out T14 t14)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                    point += Ipm10.UnPack(point, out t10);
                    point += Ipm11.UnPack(point, out t11);
                    point += Ipm12.UnPack(point, out t12);
                    point += Ipm13.UnPack(point, out t13);
                    Ipm14.UnPack(point, out t14);
                }
            }
        }
#endregion
#region M15
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;
            private readonly IPackagerMethod<T10> Ipm10;
            private readonly IPackagerMethod<T11> Ipm11;
            private readonly IPackagerMethod<T12> Ipm12;
            private readonly IPackagerMethod<T13> Ipm13;
            private readonly IPackagerMethod<T14> Ipm14;
            private readonly IPackagerMethod<T15> Ipm15;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9,
                IPackagerMethod<T10> ipm10,
                IPackagerMethod<T11> ipm11,
                IPackagerMethod<T12> ipm12,
                IPackagerMethod<T13> ipm13,
                IPackagerMethod<T14> ipm14,
                IPackagerMethod<T15> ipm15)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
                Ipm10 = ipm10;
                Ipm11 = ipm11;
                Ipm12 = ipm12;
                Ipm13 = ipm13;
                Ipm14 = ipm14;
                Ipm15 = ipm15;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9) +
                       Ipm10.GetSize(t10) +
                       Ipm11.GetSize(t11) +
                       Ipm12.GetSize(t12) +
                       Ipm13.GetSize(t13) +
                       Ipm14.GetSize(t14) +
                       Ipm15.GetSize(t15);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    Ipm15.PackUP(point, t15);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    Ipm15.PackUP(point, t15);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9, out T10 t10, out T11 t11, out T12 t12, out T13 t13, out T14 t14, out T15 t15)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                    point += Ipm10.UnPack(point, out t10);
                    point += Ipm11.UnPack(point, out t11);
                    point += Ipm12.UnPack(point, out t12);
                    point += Ipm13.UnPack(point, out t13);
                    point += Ipm14.UnPack(point, out t14);
                    Ipm15.UnPack(point, out t15);
                }
            }
        }
#endregion
#region M16
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;
            private readonly IPackagerMethod<T10> Ipm10;
            private readonly IPackagerMethod<T11> Ipm11;
            private readonly IPackagerMethod<T12> Ipm12;
            private readonly IPackagerMethod<T13> Ipm13;
            private readonly IPackagerMethod<T14> Ipm14;
            private readonly IPackagerMethod<T15> Ipm15;
            private readonly IPackagerMethod<T16> Ipm16;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9,
                IPackagerMethod<T10> ipm10,
                IPackagerMethod<T11> ipm11,
                IPackagerMethod<T12> ipm12,
                IPackagerMethod<T13> ipm13,
                IPackagerMethod<T14> ipm14,
                IPackagerMethod<T15> ipm15,
                IPackagerMethod<T16> ipm16)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
                Ipm10 = ipm10;
                Ipm11 = ipm11;
                Ipm12 = ipm12;
                Ipm13 = ipm13;
                Ipm14 = ipm14;
                Ipm15 = ipm15;
                Ipm16 = ipm16;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9) +
                       Ipm10.GetSize(t10) +
                       Ipm11.GetSize(t11) +
                       Ipm12.GetSize(t12) +
                       Ipm13.GetSize(t13) +
                       Ipm14.GetSize(t14) +
                       Ipm15.GetSize(t15) +
                       Ipm16.GetSize(t16);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    point += Ipm15.PackUP(point, t15);
                    Ipm16.PackUP(point, t16);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    point += Ipm15.PackUP(point, t15);
                    Ipm16.PackUP(point, t16);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9, out T10 t10, out T11 t11, out T12 t12, out T13 t13, out T14 t14, out T15 t15, out T16 t16)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                    point += Ipm10.UnPack(point, out t10);
                    point += Ipm11.UnPack(point, out t11);
                    point += Ipm12.UnPack(point, out t12);
                    point += Ipm13.UnPack(point, out t13);
                    point += Ipm14.UnPack(point, out t14);
                    point += Ipm15.UnPack(point, out t15);
                    Ipm16.UnPack(point, out t16);
                }
            }
        }
#endregion
#region M17
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;
            private readonly IPackagerMethod<T10> Ipm10;
            private readonly IPackagerMethod<T11> Ipm11;
            private readonly IPackagerMethod<T12> Ipm12;
            private readonly IPackagerMethod<T13> Ipm13;
            private readonly IPackagerMethod<T14> Ipm14;
            private readonly IPackagerMethod<T15> Ipm15;
            private readonly IPackagerMethod<T16> Ipm16;
            private readonly IPackagerMethod<T17> Ipm17;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9,
                IPackagerMethod<T10> ipm10,
                IPackagerMethod<T11> ipm11,
                IPackagerMethod<T12> ipm12,
                IPackagerMethod<T13> ipm13,
                IPackagerMethod<T14> ipm14,
                IPackagerMethod<T15> ipm15,
                IPackagerMethod<T16> ipm16,
                IPackagerMethod<T17> ipm17)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
                Ipm10 = ipm10;
                Ipm11 = ipm11;
                Ipm12 = ipm12;
                Ipm13 = ipm13;
                Ipm14 = ipm14;
                Ipm15 = ipm15;
                Ipm16 = ipm16;
                Ipm17 = ipm17;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9) +
                       Ipm10.GetSize(t10) +
                       Ipm11.GetSize(t11) +
                       Ipm12.GetSize(t12) +
                       Ipm13.GetSize(t13) +
                       Ipm14.GetSize(t14) +
                       Ipm15.GetSize(t15) +
                       Ipm16.GetSize(t16) +
                       Ipm17.GetSize(t17);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    point += Ipm15.PackUP(point, t15);
                    point += Ipm16.PackUP(point, t16);
                    Ipm17.PackUP(point, t17);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    point += Ipm15.PackUP(point, t15);
                    point += Ipm16.PackUP(point, t16);
                    Ipm17.PackUP(point, t17);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9, out T10 t10, out T11 t11, out T12 t12, out T13 t13, out T14 t14, out T15 t15, out T16 t16, out T17 t17)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                    point += Ipm10.UnPack(point, out t10);
                    point += Ipm11.UnPack(point, out t11);
                    point += Ipm12.UnPack(point, out t12);
                    point += Ipm13.UnPack(point, out t13);
                    point += Ipm14.UnPack(point, out t14);
                    point += Ipm15.UnPack(point, out t15);
                    point += Ipm16.UnPack(point, out t16);
                    Ipm17.UnPack(point, out t17);
                }
            }
        }
#endregion
#region M18
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;
            private readonly IPackagerMethod<T10> Ipm10;
            private readonly IPackagerMethod<T11> Ipm11;
            private readonly IPackagerMethod<T12> Ipm12;
            private readonly IPackagerMethod<T13> Ipm13;
            private readonly IPackagerMethod<T14> Ipm14;
            private readonly IPackagerMethod<T15> Ipm15;
            private readonly IPackagerMethod<T16> Ipm16;
            private readonly IPackagerMethod<T17> Ipm17;
            private readonly IPackagerMethod<T18> Ipm18;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9,
                IPackagerMethod<T10> ipm10,
                IPackagerMethod<T11> ipm11,
                IPackagerMethod<T12> ipm12,
                IPackagerMethod<T13> ipm13,
                IPackagerMethod<T14> ipm14,
                IPackagerMethod<T15> ipm15,
                IPackagerMethod<T16> ipm16,
                IPackagerMethod<T17> ipm17,
                IPackagerMethod<T18> ipm18)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
                Ipm10 = ipm10;
                Ipm11 = ipm11;
                Ipm12 = ipm12;
                Ipm13 = ipm13;
                Ipm14 = ipm14;
                Ipm15 = ipm15;
                Ipm16 = ipm16;
                Ipm17 = ipm17;
                Ipm18 = ipm18;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9) +
                       Ipm10.GetSize(t10) +
                       Ipm11.GetSize(t11) +
                       Ipm12.GetSize(t12) +
                       Ipm13.GetSize(t13) +
                       Ipm14.GetSize(t14) +
                       Ipm15.GetSize(t15) +
                       Ipm16.GetSize(t16) +
                       Ipm17.GetSize(t17) +
                       Ipm18.GetSize(t18);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    point += Ipm15.PackUP(point, t15);
                    point += Ipm16.PackUP(point, t16);
                    point += Ipm17.PackUP(point, t17);
                    Ipm18.PackUP(point, t18);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    point += Ipm15.PackUP(point, t15);
                    point += Ipm16.PackUP(point, t16);
                    point += Ipm17.PackUP(point, t17);
                    Ipm18.PackUP(point, t18);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9, out T10 t10, out T11 t11, out T12 t12, out T13 t13, out T14 t14, out T15 t15, out T16 t16, out T17 t17, out T18 t18)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                    point += Ipm10.UnPack(point, out t10);
                    point += Ipm11.UnPack(point, out t11);
                    point += Ipm12.UnPack(point, out t12);
                    point += Ipm13.UnPack(point, out t13);
                    point += Ipm14.UnPack(point, out t14);
                    point += Ipm15.UnPack(point, out t15);
                    point += Ipm16.UnPack(point, out t16);
                    point += Ipm17.UnPack(point, out t17);
                    Ipm18.UnPack(point, out t18);
                }
            }
        }
#endregion
#region M19
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;
            private readonly IPackagerMethod<T10> Ipm10;
            private readonly IPackagerMethod<T11> Ipm11;
            private readonly IPackagerMethod<T12> Ipm12;
            private readonly IPackagerMethod<T13> Ipm13;
            private readonly IPackagerMethod<T14> Ipm14;
            private readonly IPackagerMethod<T15> Ipm15;
            private readonly IPackagerMethod<T16> Ipm16;
            private readonly IPackagerMethod<T17> Ipm17;
            private readonly IPackagerMethod<T18> Ipm18;
            private readonly IPackagerMethod<T19> Ipm19;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9,
                IPackagerMethod<T10> ipm10,
                IPackagerMethod<T11> ipm11,
                IPackagerMethod<T12> ipm12,
                IPackagerMethod<T13> ipm13,
                IPackagerMethod<T14> ipm14,
                IPackagerMethod<T15> ipm15,
                IPackagerMethod<T16> ipm16,
                IPackagerMethod<T17> ipm17,
                IPackagerMethod<T18> ipm18,
                IPackagerMethod<T19> ipm19)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
                Ipm10 = ipm10;
                Ipm11 = ipm11;
                Ipm12 = ipm12;
                Ipm13 = ipm13;
                Ipm14 = ipm14;
                Ipm15 = ipm15;
                Ipm16 = ipm16;
                Ipm17 = ipm17;
                Ipm18 = ipm18;
                Ipm19 = ipm19;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9) +
                       Ipm10.GetSize(t10) +
                       Ipm11.GetSize(t11) +
                       Ipm12.GetSize(t12) +
                       Ipm13.GetSize(t13) +
                       Ipm14.GetSize(t14) +
                       Ipm15.GetSize(t15) +
                       Ipm16.GetSize(t16) +
                       Ipm17.GetSize(t17) +
                       Ipm18.GetSize(t18) +
                       Ipm19.GetSize(t19);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    point += Ipm15.PackUP(point, t15);
                    point += Ipm16.PackUP(point, t16);
                    point += Ipm17.PackUP(point, t17);
                    point += Ipm18.PackUP(point, t18);
                    Ipm19.PackUP(point, t19);

                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    point += Ipm15.PackUP(point, t15);
                    point += Ipm16.PackUP(point, t16);
                    point += Ipm17.PackUP(point, t17);
                    point += Ipm18.PackUP(point, t18);
                    Ipm19.PackUP(point, t19);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9, out T10 t10, out T11 t11, out T12 t12, out T13 t13, out T14 t14, out T15 t15, out T16 t16, out T17 t17, out T18 t18, out T19 t19)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                    point += Ipm10.UnPack(point, out t10);
                    point += Ipm11.UnPack(point, out t11);
                    point += Ipm12.UnPack(point, out t12);
                    point += Ipm13.UnPack(point, out t13);
                    point += Ipm14.UnPack(point, out t14);
                    point += Ipm15.UnPack(point, out t15);
                    point += Ipm16.UnPack(point, out t16);
                    point += Ipm17.UnPack(point, out t17);
                    point += Ipm18.UnPack(point, out t18);
                    Ipm19.UnPack(point, out t19);
                }
            }
        }
#endregion
#region M20
        /// <summary>
        /// Packer
        /// </summary>
        public class M<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>
        {
            private readonly IPackagerMethod<T1> Ipm1;
            private readonly IPackagerMethod<T2> Ipm2;
            private readonly IPackagerMethod<T3> Ipm3;
            private readonly IPackagerMethod<T4> Ipm4;
            private readonly IPackagerMethod<T5> Ipm5;
            private readonly IPackagerMethod<T6> Ipm6;
            private readonly IPackagerMethod<T7> Ipm7;
            private readonly IPackagerMethod<T8> Ipm8;
            private readonly IPackagerMethod<T9> Ipm9;
            private readonly IPackagerMethod<T10> Ipm10;
            private readonly IPackagerMethod<T11> Ipm11;
            private readonly IPackagerMethod<T12> Ipm12;
            private readonly IPackagerMethod<T13> Ipm13;
            private readonly IPackagerMethod<T14> Ipm14;
            private readonly IPackagerMethod<T15> Ipm15;
            private readonly IPackagerMethod<T16> Ipm16;
            private readonly IPackagerMethod<T17> Ipm17;
            private readonly IPackagerMethod<T18> Ipm18;
            private readonly IPackagerMethod<T19> Ipm19;
            private readonly IPackagerMethod<T20> Ipm20;

            internal M(
                IPackagerMethod<T1> ipm1,
                IPackagerMethod<T2> ipm2,
                IPackagerMethod<T3> ipm3,
                IPackagerMethod<T4> ipm4,
                IPackagerMethod<T5> ipm5,
                IPackagerMethod<T6> ipm6,
                IPackagerMethod<T7> ipm7,
                IPackagerMethod<T8> ipm8,
                IPackagerMethod<T9> ipm9,
                IPackagerMethod<T10> ipm10,
                IPackagerMethod<T11> ipm11,
                IPackagerMethod<T12> ipm12,
                IPackagerMethod<T13> ipm13,
                IPackagerMethod<T14> ipm14,
                IPackagerMethod<T15> ipm15,
                IPackagerMethod<T16> ipm16,
                IPackagerMethod<T17> ipm17,
                IPackagerMethod<T18> ipm18,
                IPackagerMethod<T19> ipm19,
                IPackagerMethod<T20> ipm20)
            {
                Ipm1 = ipm1;
                Ipm2 = ipm2;
                Ipm3 = ipm3;
                Ipm4 = ipm4;
                Ipm5 = ipm5;
                Ipm6 = ipm6;
                Ipm7 = ipm7;
                Ipm8 = ipm8;
                Ipm9 = ipm9;
                Ipm10 = ipm10;
                Ipm11 = ipm11;
                Ipm12 = ipm12;
                Ipm13 = ipm13;
                Ipm14 = ipm14;
                Ipm15 = ipm15;
                Ipm16 = ipm16;
                Ipm17 = ipm17;
                Ipm18 = ipm18;
                Ipm19 = ipm19;
                Ipm20 = ipm20;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public int CalcNeedSize(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19, T20 t20)
            {
                return Ipm1.GetSize(t1) +
                       Ipm2.GetSize(t2) +
                       Ipm3.GetSize(t3) +
                       Ipm4.GetSize(t4) +
                       Ipm5.GetSize(t5) +
                       Ipm6.GetSize(t6) +
                       Ipm7.GetSize(t7) +
                       Ipm8.GetSize(t8) +
                       Ipm9.GetSize(t9) +
                       Ipm10.GetSize(t10) +
                       Ipm11.GetSize(t11) +
                       Ipm12.GetSize(t12) +
                       Ipm13.GetSize(t13) +
                       Ipm14.GetSize(t14) +
                       Ipm15.GetSize(t15) +
                       Ipm16.GetSize(t16) +
                       Ipm17.GetSize(t17) +
                       Ipm18.GetSize(t18) +
                       Ipm19.GetSize(t19) +
                       Ipm20.GetSize(t20);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, int offset, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19, T20 t20)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    point += Ipm15.PackUP(point, t15);
                    point += Ipm16.PackUP(point, t16);
                    point += Ipm17.PackUP(point, t17);
                    point += Ipm18.PackUP(point, t18);
                    point += Ipm19.PackUP(point, t19);
                    Ipm20.PackUP(point, t20);

                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19, T20 t20)
            {
                byte[] destination = new byte[CalcNeedSize(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    point += Ipm1.PackUP(point, t1);
                    point += Ipm2.PackUP(point, t2);
                    point += Ipm3.PackUP(point, t3);
                    point += Ipm4.PackUP(point, t4);
                    point += Ipm5.PackUP(point, t5);
                    point += Ipm6.PackUP(point, t6);
                    point += Ipm7.PackUP(point, t7);
                    point += Ipm8.PackUP(point, t8);
                    point += Ipm9.PackUP(point, t9);
                    point += Ipm10.PackUP(point, t10);
                    point += Ipm11.PackUP(point, t11);
                    point += Ipm12.PackUP(point, t12);
                    point += Ipm13.PackUP(point, t13);
                    point += Ipm14.PackUP(point, t14);
                    point += Ipm15.PackUP(point, t15);
                    point += Ipm16.PackUP(point, t16);
                    point += Ipm17.PackUP(point, t17);
                    point += Ipm18.PackUP(point, t18);
                    point += Ipm19.PackUP(point, t19);
                    Ipm20.PackUP(point, t20);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, int offset, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5, out T6 t6, out T7 t7, out T8 t8, out T9 t9, out T10 t10, out T11 t11, out T12 t12, out T13 t13, out T14 t14, out T15 t15, out T16 t16, out T17 t17, out T18 t18, out T19 t19, out T20 t20)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    point += Ipm1.UnPack(point, out t1);
                    point += Ipm2.UnPack(point, out t2);
                    point += Ipm3.UnPack(point, out t3);
                    point += Ipm4.UnPack(point, out t4);
                    point += Ipm5.UnPack(point, out t5);
                    point += Ipm6.UnPack(point, out t6);
                    point += Ipm7.UnPack(point, out t7);
                    point += Ipm8.UnPack(point, out t8);
                    point += Ipm9.UnPack(point, out t9);
                    point += Ipm10.UnPack(point, out t10);
                    point += Ipm11.UnPack(point, out t11);
                    point += Ipm12.UnPack(point, out t12);
                    point += Ipm13.UnPack(point, out t13);
                    point += Ipm14.UnPack(point, out t14);
                    point += Ipm15.UnPack(point, out t15);
                    point += Ipm16.UnPack(point, out t16);
                    point += Ipm17.UnPack(point, out t17);
                    point += Ipm18.UnPack(point, out t18);
                    point += Ipm19.UnPack(point, out t19);
                    Ipm20.UnPack(point, out t20);
                }
            }
        }
#endregion
#endif
    }
 }