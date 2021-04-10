using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmartPackager.BasicPackMethods
{
    /// <summary>
    /// allows you to get a PackStructUnmanaged of the desired type
    /// </summary>
    public static class PackStructUnmanagedExtension
    {
        private static MethodInfo GetPackFromType_MethodInfo => typeof(PackStructUnmanagedExtension).GetMethod("GetPackFromType", BindingFlags.NonPublic | BindingFlags.Static);
        internal static Dictionary<Type, IPackagerMethod> Cash = new Dictionary<Type, IPackagerMethod>();

        /// <summary>
        /// Allows you to get a PackStructUnmanaged of the desired type 
        /// *[has caching of generated types]
        /// </summary>
        /// <param name="type"> target type (unmanaged)</param>
        /// <returns></returns>
        public static IPackagerMethod Make(Type type)
        {
            if (Cash.TryGetValue(type, out IPackagerMethod packager))
            {
                return packager;
            }

            if (type.IsUnManaged() == false)
                throw new Exception("This type is not unmanaged!");

            MethodInfo mi = GetPackFromType_MethodInfo.MakeGenericMethod(type);

            IPackagerMethod pack = (IPackagerMethod)mi.Invoke(null, null);
            Cash.Add(type, pack);
            return pack;
        }

        private static IPackagerMethod GetPackFromType<T>() where T : unmanaged
        {
            return new PackStructUnmanaged<T>();
        }
    }

    public class PackStructUnmanaged<T> : IPackagerMethod where T : unmanaged
    {
        public Type TargetType => typeof(T);

        internal PackStructUnmanaged()
        {

        }

        public unsafe long PackUP(byte* destination, object source)
        {
            *(T*)destination = (T)source;
            return sizeof(T);
        }

        public unsafe long UnPack(byte* source, out object destination)
        {
            T pt = *(T*)source;
            destination = pt;
            return sizeof(T);
        }

        public unsafe long GetSize(object source)
        {
            return sizeof(T);
        }
    }
}
