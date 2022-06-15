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
        /// <summary>
        /// Закешированные упаковщики
        /// </summary>
        internal static Dictionary<Type, IPackagerMethodGeneric> PackMethodsDictionary = new Dictionary<Type, IPackagerMethodGeneric>();
        internal static Dictionary<string, Type> PackGenericNoCreatedMethodsDictionary = new Dictionary<string, Type>();

        internal static string GetFullName(this Type type)
        {
            return type.Namespace + ":" + type.Name;
        }

        /// <summary>
        /// loads data type packaging implementations
        /// </summary>
        /// <returns></returns>
        internal static void SetupPackMethods()
        {
            PackMethodsDictionary.Clear();
            PackGenericNoCreatedMethodsDictionary.Clear();

            //find IPackagerMethods
            Type targetFind = typeof(IPackagerMethodGeneric);
            System.Reflection.Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> IPackagerMethods = new List<Type>(1000);
            for (int i = 0; i < Assemblies.Length; i++)
            {
                try
                {
                    Type[] types = Assemblies[i].GetTypes();
                    for (int j = 0; j < types.Length; j++)
                    {
                        if (targetFind.IsAssignableFrom(types[j]) && targetFind != types[j])
                        {
                            IPackagerMethods.Add(types[j]);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("SmartPackager -> Fail load Assembly -> " + Assemblies[i].Location);
                }

            }
            //var IPackagerMethods = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => targetFind.IsAssignableFrom(p) && targetFind != p);

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
                        PackMethodsDictionary.Add(ipm.TargetType, ipm);
                    }
                    catch
                    {
                        throw new Exception($"A packaging implementation for this type already exists!\ninfo\nType: {ipm.TargetType.FullName}\nClass: {ipm.GetType().FullName}");
                    }
                }
                else
                {


                    var iPackagerMethod = TypeIpm.GetInterface("IPackagerMethod`1");
                    if (iPackagerMethod != null)
                    {
                        try
                        {
                            var genericType = iPackagerMethod.GenericTypeArguments[0];
                            if (genericType.GenericTypeArguments.Length > 0)
                            {
                                string fullName = genericType.GetFullName();
                                PackGenericNoCreatedMethodsDictionary.Add(fullName, TypeIpm);
                            }
                        }
                        catch
                        {
                            throw new Exception($"A packaging implementation for this type already exists!\ninfo\nType: {TypeIpm.FullName}\nClass: {TypeIpm.FullName}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// if any of the packaging classes were skipped you can start the search function again
        /// </summary>
        public static void SetupAgainPackMethods()
        {
            SetupPackMethods();
            Packager.SetupDone = true;
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