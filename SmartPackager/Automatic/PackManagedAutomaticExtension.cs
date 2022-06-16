using System;
using System.Reflection;
using System.Collections.Generic;

namespace SmartPackager.Automatic
{
    internal static class PackManagedAutomaticExtension
    {
        /// <summary>
        /// Хранит уже созданные упаковщики
        /// </summary>
        internal static readonly Dictionary<Type, IPackagerMethodGeneric> Cash = new Dictionary<Type, IPackagerMethodGeneric>();

        /// <summary>
        /// Tries to create an unpacking method for a managed type
        /// </summary>
        /// <typeparam name="T">target type</typeparam>
        /// <returns></returns>
        public static IPackagerMethodGeneric Make<T>()
        {
            lock (Cash)
            {
                if (Cash.TryGetValue(typeof(T), out IPackagerMethodGeneric packager))
                {
                    return packager;
                }

                if (typeof(T).IsUnManaged() == true)
                    throw new Exception("This type is unmanaged!");

                IPackagerMethodGeneric pack = GetPackForType<T>();
                Cash.Add(typeof(T), pack);
                return pack;
            }
        }

        /// <summary>
        /// Кэш для объектов которые находяться на этапе создания (что бы избавиться от рекурсивного создания упаковщика)
        /// </summary>
        public static readonly Dictionary<Type, IPackagerMethodGeneric> CreatingObjectCash = new Dictionary<Type, IPackagerMethodGeneric>(10);

        /// <summary>
        /// Создаёт упаковщик для указанного управляймого типа (не кэшеирует результат)
        /// </summary>
        /// <typeparam name="T">тип для которого будет создан упаковщик</typeparam>
        /// <returns>упаковщик</returns>
        private static IPackagerMethodGeneric GetPackForType<T>()
        {
            var pack = new PackManagedAutomatic<T>();
            try
            {
                CreatingObjectCash.Add(typeof(T), pack);

                if (typeof(T).IsArray)
                {
                    PackManagedAutomaticExtension_PackArray.Make(pack);
                }
                else
                {
                    PackManagedAutomaticExtension_Container.Make(pack);
                }
            }
            finally
            {
                CreatingObjectCash.Remove(typeof(T));
            }
            return pack;
        }
    }
}