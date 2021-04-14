﻿namespace SmartPackager
{
    /// <summary>
    /// Creates a packer for the selected set of types
    /// </summary>
    public static class Packager
    {
        internal static IPackagerMethod<T> GetMethods<T>()
        {
            //searches for a method implementation for this type or tries to generate
            if (PackMethods.PackMethodsDictionary.TryGetValue(typeof(T), out IPackagerMethodGeneric ipm))
            {
                return (IPackagerMethod<T>)ipm;
            }
            else if (typeof(T).IsUnManaged())
            {
                return (IPackagerMethod<T>)BasicPackMethods.PackStructUnmanagedAutomaticExtension.Make<T>();
            }
            else
            {
                return (IPackagerMethod<T>)BasicPackMethods.PackStructManagedAutomaticExtension.Make<T>();
            }
        }

        /// <summary>
        /// Creates a packer for the selected set of types
        /// </summary>
        /// <returns>packer</returns>
        public static M1<T1> Create<T1>()
        {
            return new M1<T1>(GetMethods<T1>());
        }
        #region M1

        /// <summary>
        /// Packer
        /// </summary>
        public class M1<T1>
        {
            private readonly IPackagerMethod<T1> Ipm1;

            internal M1(
                IPackagerMethod<T1> ipm1)
            {
                Ipm1 = ipm1;
            }
            /// <summary>
            /// Сalculates the required array size for packing
            /// </summary>
            /// <returns></returns>
            public long CalcNeedSize(T1 t1)
            {
                return Ipm1.GetSize(t1);
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe void PackUP(byte[] destination, long offset, T1 t1)
            {
                fixed (byte* dest = &destination[offset])
                {
                    byte* point = dest;
                    Ipm1.PackUP(point, t1);
                }
            }
            /// <summary>
            /// Packs data into an array
            /// </summary>
            public unsafe byte[] PackUP(T1 t1)
            {
                byte[] destination = new byte[CalcNeedSize(t1)];
                fixed (byte* dest = &destination[0])
                {
                    byte* point = dest;
                    Ipm1.PackUP(point, t1);
                }
                return destination;
            }
            /// <summary>
            /// Unpacks data from an array
            /// </summary>
            public unsafe void UnPack(byte[] source, long offset, out T1 t1)
            {
                fixed (byte* dest = &source[offset])
                {
                    byte* point = dest;
                    Ipm1.UnPack(point, out t1);
                }
            }
        }
        #endregion
    }
}