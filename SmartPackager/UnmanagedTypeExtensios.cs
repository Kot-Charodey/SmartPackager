using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartPackager
{
    /// <summary>
    /// Сhecks if the type is managed
    /// </summary>
    public static class UnmanagedTypeExtensios
    {
        private static Dictionary<Type, bool> cachedTypes = new Dictionary<Type, bool>();

        /// <summary>
        /// Сhecks if the type is managed
        /// </summary>
        /// <param name="t"></param>
        /// <returns>returned true if type is unmanaged</returns>
        public static bool IsUnManaged(this Type t)
        {
            bool result = false;
            if (cachedTypes.TryGetValue(t, out result))
                return result;
            else if (t.IsPrimitive || t.IsPointer || t.IsEnum)
                result = true;
            else if (t.IsGenericType || !t.IsValueType)
                result = false;
            else
                result = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).
                    All(x => x.FieldType.IsUnManaged());
            cachedTypes.Add(t, result);
            return result;
        }
    }
}