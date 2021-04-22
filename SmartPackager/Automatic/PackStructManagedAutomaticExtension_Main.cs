using System;
using System.Reflection;
using System.Collections.Generic;

namespace SmartPackager.Automatic
{
    internal static partial class PackStructManagedAutomaticExtension
    {
        internal static readonly Dictionary<Type, IPackagerMethodGeneric> Cash = new Dictionary<Type, IPackagerMethodGeneric>();

        internal unsafe delegate long Delegate_GetSize<T>(T source);
        internal unsafe delegate long Delegate_PackUP<T>(byte* destination, T source);
        internal unsafe delegate long Delegate_UnPack<T>(byte* source, out T destination);

        private delegate MethodsData<T> delegate_PackGenerator<T>();

        private struct MethodsData<T>
        {
            public Delegate_GetSize<T> action_GetSize;
            public Delegate_PackUP<T> action_PackUP;
            public Delegate_UnPack<T> action_UnPack;
            public bool isFixedSize;
        }


        private static MethodInfo PackArray_MethodInfo => typeof(PackStructManagedAutomaticExtension).GetMethod("PackArray", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// Tries to create an unpacking method for a managed type
        /// </summary>
        /// <typeparam name="T">target type</typeparam>
        /// <returns></returns>
        public static IPackagerMethodGeneric Make<T>()
        {
            if (Cash.TryGetValue(typeof(T), out IPackagerMethodGeneric packager))
            {
                return packager;
            }

            if (typeof(T).IsUnManaged() == true)
                throw new Exception("This type is unmanaged!");

            IPackagerMethodGeneric pack = GetPackForType<T>();

            Cash.Add(typeof(T), pack);
            return pack;
        }

        private static IPackagerMethodGeneric GetPackForType<T>()
        {
            MethodsData<T> data;

            if (typeof(T).IsArray)
            {
                MethodInfo mi = PackArray_MethodInfo.MakeGenericMethod(typeof(T), typeof(T).GetElementType());
                delegate_PackGenerator<T> dpa = (delegate_PackGenerator<T>)mi.CreateDelegate(typeof(delegate_PackGenerator<T>));

                data = dpa.Invoke();
            }
            else
            {
                data = PackContainer<T>();
            }

            return new PackStructManagedAutomatic<T>(data.action_GetSize, data.action_PackUP, data.action_UnPack, data.isFixedSize);
        }
    }
}
