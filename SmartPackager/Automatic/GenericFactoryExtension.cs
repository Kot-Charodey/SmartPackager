using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPackager.Automatic
{
    internal static class GenericFactoryExtension
    {
        /// <summary>
        /// Кэш уже инициализированных вариантов универсальных типов типов
        /// </summary>
        public static Dictionary<Type, IPackagerMethodGeneric> Cache = new Dictionary<Type, IPackagerMethodGeneric>();

        /// <summary>
        /// Создает вариант универсального типа
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IPackagerMethodGeneric Make<T>(Type type) {
            
            Type GenericType = type.MakeGenericType(typeof(T).GetGenericArguments());

            if (!(Activator.CreateInstance(GenericType) is IPackagerMethodGeneric ipm))
            {
                throw new Exception("Failed to create " + nameof(T));
            }

            Cache.Add(GenericType, ipm);
            return ipm;
        }
    }
}
