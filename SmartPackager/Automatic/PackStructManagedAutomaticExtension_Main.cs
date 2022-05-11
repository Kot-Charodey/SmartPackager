using System;
using System.Reflection;
using System.Collections.Generic;

namespace SmartPackager.Automatic
{
    internal static partial class PackStructManagedAutomaticExtension
    {
        internal static readonly Dictionary<Type, IPackagerMethodGeneric> Cash = new Dictionary<Type, IPackagerMethodGeneric>();

        internal unsafe delegate int Delegate_GetSize<T>(T source);
        internal unsafe delegate int Delegate_PackUP<T>(byte* destination, T source);
        internal unsafe delegate int Delegate_UnPack<T>(byte* source, out T destination);

        private delegate MethodsDataHeap<T> delegate_PackGenerator<T>();

        internal unsafe delegate int Delegate_GetSizeHeap<T>(T source, ManagedHeap heap);
        internal unsafe delegate int Delegate_PackUPHeap<T>(byte* destination, ManagedHeap heap, T source);
        internal unsafe delegate int Delegate_UnPackHeap<T>(byte* source, ManagedHeap heap, out T destination);

        internal enum PointerType : byte
        {
            Error,
            Null,
            Data,
            Pointer,
        }

        internal struct MethodsDataHeap<T>
        {
            public Delegate_GetSizeHeap<T> Action_GetSize;
            public Delegate_PackUPHeap<T> Action_PackUP;
            public Delegate_UnPackHeap<T> Action_UnPack;
            public bool IsFixedSize;
        }


        private static MethodInfo PackArray_MethodInfo => typeof(PackStructManagedAutomaticExtension).GetMethod("PackArray", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// Tries to create an unpacking method for a managed type
        /// </summary>
        /// <typeparam name="T">target type</typeparam>
        /// <returns></returns>
        public static IPackagerMethodGeneric Make<T>()
        {
            lock (Cash)
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
        }

        /// <summary>
        /// contains packers that have not yet been created to the end
        /// enables recursive creation of the packager
        /// </summary>
        private static readonly Dictionary<Type,object> CreatingPackagerCash = new Dictionary<Type,object>();

        private static IPackagerMethodGeneric GetPackForType<T>()
        {
            var thisPackager = new RefObject<PackStructManagedAutomaticHeap<T>>();
            CreatingPackagerCash.Add(typeof(T), thisPackager);
            MethodsDataHeap<T> data;

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


            var psma = new PackStructManagedAutomaticHeap<T>(data);
            thisPackager.Object = psma;
            CreatingPackagerCash.Remove(typeof(T));
            return psma;

        }
    }
}