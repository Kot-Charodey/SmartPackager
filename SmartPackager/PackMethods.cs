using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartPackager
{
    public static class PackMethods
    {
        internal static Dictionary<Type, IPackagerMethod> PackMethodsDictionary = SetupPackMethods();

        /// <summary>
        /// loads data type packaging implementations
        /// </summary>
        /// <returns></returns>
        private static Dictionary<Type, IPackagerMethod> SetupPackMethods()
        {
            Dictionary<Type, IPackagerMethod> sm = new Dictionary<Type, IPackagerMethod>();

            //find IPackagerMethods
            Type targetFind = typeof(IPackagerMethod);
            var IPackagerMethods = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => targetFind.IsAssignableFrom(p) && targetFind != p);

            foreach (Type TypeIpm in IPackagerMethods)
            {
                if (!TypeIpm.IsGenericType)
                {
                    IPackagerMethod ipm = Activator.CreateInstance(TypeIpm) as IPackagerMethod;
                    if (ipm == null)
                    {
                        throw new Exception("Failed to create " + TypeIpm.FullName);
                    }

                    try
                    {
                        sm.Add(ipm.TargetType, ipm);
                    }
                    catch
                    {
                        throw new Exception($"a packaging implementation for this type already exists!\ninfo\nType: {ipm.TargetType.FullName}\nClass: {ipm.GetType().FullName}");
                    }
                }
            }

            return sm;
        }

        /// <summary>
        /// if any of the packaging classes were skipped you can start the search function again
        /// </summary>
        public static void SetupAgainPackMethods()
        {
            PackMethodsDictionary = SetupPackMethods();
        }

        /// <summary>
        /// returns all packing methods *[unmanaged types generated automatically are not included]
        /// </summary>
        /// <returns>all packing methods</returns>
        public static IPackagerMethod[] GetPackMethods()
        {
            return PackMethodsDictionary.Values.ToArray();
        }

        /// <summary>
        /// Unmanaged types generated automatically
        /// </summary>
        /// <returns></returns>
        public static IPackagerMethod[] GetPackagerMethodsUnmanagedTypes()
        {
            return BasicPackMethods.PackStructUnmanagedExtension.Cash.Values.ToArray();
        }
    }
}
