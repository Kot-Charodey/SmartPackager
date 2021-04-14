using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmartPackager.BasicPackMethods
{
    /// <summary>
    /// allows you to get a PackStructUnmanaged of the desired type
    /// </summary>
    internal static class PackStructUnmanagedAutomaticExtension
    {
        private static MethodInfo GetPackFromType_MethodInfo => typeof(PackStructUnmanagedAutomaticExtension).GetMethod("GetPackFromType", BindingFlags.NonPublic | BindingFlags.Static);
        internal static Dictionary<Type, IPackagerMethodGeneric> Cash = new Dictionary<Type, IPackagerMethodGeneric>();

        /// <summary>
        /// Allows you to get a PackStructUnmanaged of the desired type 
        /// *[has caching of generated types]
        /// </summary>
        /// <returns></returns>
        public static IPackagerMethodGeneric Make<T>()
        {
            if (Cash.TryGetValue(typeof(T), out IPackagerMethodGeneric packager))
            {
                return packager;
            }

            if (typeof(T).IsUnManaged() == false)
                throw new Exception("This type is not unmanaged!");

            //exception bypass - this type must be unmanaged
            MethodInfo mi = GetPackFromType_MethodInfo.MakeGenericMethod(typeof(T));
            IPackagerMethodGeneric pack = (IPackagerMethodGeneric)mi.Invoke(null, null);

            Cash.Add(typeof(T), pack);
            return pack;
        }

        private static IPackagerMethodGeneric GetPackFromType<T>() where T : unmanaged
        {
            return new PackStructUnmanagedAutomatic<T>();
        }
    }
}
