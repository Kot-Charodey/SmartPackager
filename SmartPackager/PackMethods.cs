using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartPackager
{
    /// <summary>
    /// Выполняет поиск и инициализацию методов упаковки
    /// </summary>
    public static class PackMethods
    {
        /// <summary>
        /// Реализованные в ручную упаковщики типов (заполняется поиском через рефлексию)
        /// </summary>
        internal static Dictionary<Type, IPackagerMethodGeneric> PackMethodsDictionary = new Dictionary<Type, IPackagerMethodGeneric>();
        /// <summary>
        /// Реализованные в ручную универсальные типы, их необходимо инициализировать под определённый тип (заполняется поиском через рефлексию)
        /// </summary>
        internal static Dictionary<string, Type> PackGenericNoCreatedMethodsDictionary = new Dictionary<string, Type>();

        internal static string GetFullName(this Type type)
        {
            return type.Namespace + ":" + type.Name;
        }

        /// <summary>
        /// загружает реализации упаковки типов данных
        /// </summary>
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
                //если это упаковщик универсального типа
                {
                    //TODO исключить от сюда 
                    //PackUnmanagedAutomatic
                    //PackManagedAutomatic
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
        /// если какой-либо из классов упаковки был пропущен, вы можете снова запустить функцию поиска
        /// </summary>
        public static void SetupAgainPackMethods()
        {
            SetupPackMethods();
            Packager.SetupDone = true;
        }

        /// <summary>
        /// возвращает все методы упаковки * [неуправляемые типы, созданные автоматически, не включены]
        /// </summary>
        /// <returns>all packing methods</returns>
        public static IPackagerMethodGeneric[] GetPackMethods()
        {
            return PackMethodsDictionary.Values.ToArray();
        }

        /// <summary>
        /// Неуправляемые типы, генерируемые автоматически
        /// </summary>
        /// <returns></returns>
        public static IPackagerMethodGeneric[] GetPackagerMethodsUnmanagedTypes()
        {
            return Automatic.PackUnmanagedAutomaticExtension.Cash.Values.ToArray();
        }

        /// <summary>
        /// Управляемые типы генерируются автоматически
        /// </summary>
        /// <returns></returns>
        public static IPackagerMethodGeneric[] GetPackagerMethodsManagedTypes()
        {
            return Automatic.PackManagedAutomaticExtension.Cash.Values.ToArray();
        }
    }
}