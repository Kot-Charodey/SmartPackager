using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartPackager
{
    /// <summary>
    /// Searches for and initializes packaging methods
    /// </summary>
    public static class PackMethods
    {
        internal static Dictionary<Type, IPackagerMethodGeneric> PackMethodsDictionary = SetupPackMethods();

        /// <summary>
        /// loads data type packaging implementations
        /// </summary>
        /// <returns></returns>
        private static Dictionary<Type, IPackagerMethodGeneric> SetupPackMethods()
        {
            Dictionary<Type, IPackagerMethodGeneric> sm = new Dictionary<Type, IPackagerMethodGeneric>();

            //find IPackagerMethods
            Type targetFind = typeof(IPackagerMethodGeneric);
            var IPackagerMethods = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => targetFind.IsAssignableFrom(p) && targetFind != p);

            foreach (Type TypeIpm in IPackagerMethods)
            {
                if (!TypeIpm.IsGenericType)
                {
                    if (!(Activator.CreateInstance(TypeIpm) is IPackagerMethodGeneric ipm))
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
        public static IPackagerMethodGeneric[] GetPackMethods()
        {
            return PackMethodsDictionary.Values.ToArray();
        }

        /// <summary>
        /// Unmanaged types generated automatically
        /// </summary>
        /// <returns></returns>
        public static IPackagerMethodGeneric[] GetPackagerMethodsUnmanagedTypes()
        {
            return Automatic.PackStructUnmanagedAutomaticExtension.Cash.Values.ToArray();
        }

        /// <summary>
        /// Managed types generaded automatically
        /// </summary>
        /// <returns></returns>
        public static IPackagerMethodGeneric[] GetPackagerMethodsManagedTypes()
        {
            return Automatic.PackStructManagedAutomaticExtension.Cash.Values.ToArray();
        }
    }
}