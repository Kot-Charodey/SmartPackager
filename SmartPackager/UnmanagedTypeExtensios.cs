using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartPackager
{
    /// <summary>
    /// Проверяет, является ли тип управляемым
    /// </summary>
    public static class UnmanagedTypeExtensios
    {
        private static readonly Dictionary<int, bool> cachedTypes = new Dictionary<int, bool>();

        /// <summary>
        /// Проверяет, является ли тип управляемым
        /// </summary>
        /// <param name="t"></param>
        /// <returns>returned true if type is unmanaged</returns>
        public static bool IsUnManaged(this Type t)
        {
            if (cachedTypes.TryGetValue(t.FullName.GetHashCode(), out bool result))
                return result;
            else if (t.IsPrimitive || t.IsPointer || t.IsEnum)
                result = true;
            else if (t.IsGenericType || !t.IsValueType)
                result = false;
            else
                result = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).
                    All(x => x.FieldType.IsUnManaged());
            cachedTypes.Add(t.FullName.GetHashCode(), result);
            return result;
        }
    }
}