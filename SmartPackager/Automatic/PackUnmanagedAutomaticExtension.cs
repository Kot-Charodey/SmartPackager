﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmartPackager.Automatic
{
    /// <summary>
    /// позволяет получить управляемую структуру пакета нужного типа
    /// </summary>
    internal static class PackUnmanagedAutomaticExtension
    {
        private static MethodInfo GetPackFromType_MethodInfo => typeof(PackUnmanagedAutomaticExtension).GetMethod("GetPackFromType", BindingFlags.NonPublic | BindingFlags.Static);
        internal static Dictionary<Type, IPackagerMethodGeneric> Cash = new Dictionary<Type, IPackagerMethodGeneric>();

        /// <summary>
        /// Позволяет получить управляемую структуру пакета нужного типа
        /// [имеет кэширование сгенерированных типов]
        /// </summary>
        /// <returns></returns>
        public static IPackagerMethodGeneric Make<T>()
        {
            lock (Cash)
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
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Удалите неиспользуемые закрытые члены", Justification = "Reflection.Invoke")]
        private static IPackagerMethodGeneric GetPackFromType<T>() where T : unmanaged
        {
            return new PackUnmanagedAutomatic<T>();
        }
    }
}
