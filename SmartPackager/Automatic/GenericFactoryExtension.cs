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
        /// Creates a variant of the universal type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IPackagerMethodGeneric Make<T>(Type type) {
            Type GenericType = type.MakeGenericType(typeof(T).GetGenericArguments());

            if (!(Activator.CreateInstance(GenericType) is IPackagerMethodGeneric ipm))
            {
                throw new Exception("Failed to create " + nameof(T));
            }

            PackMethods.PackMethodsDictionary.Add(GenericType, ipm);
            return ipm;
        }
    }
}
