using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.InteropServices;

using static SmartPackager.BasicPackMethods.Managed.PackStructManagedAutomaticExtension;

namespace SmartPackager.BasicPackMethods.Managed
{
    public static class PackStructManagedAutomaticExtension
    {
        internal unsafe delegate long Delegate_GetSize<T>(T source);
        internal unsafe delegate long Delegate_Action_PackUP<T>(byte* destination, T sourcen);
        internal unsafe delegate long Delegate_UnPack<T>(byte* source, out T destination);

        private delegate void delegate_PackGenerator<T>(out Delegate_GetSize<T> action_GetSize, out Delegate_Action_PackUP<T> action_PackUP, out Delegate_UnPack<T> action_UnPack);

        private static MethodInfo GetMethods_MethodInfo => typeof(Pack).GetMethod("GetMethods");
        private static MethodInfo PackArray_MethodInfo => typeof(PackStructManagedAutomaticExtension).GetMethod("PackArray", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo GetPackFromType_MethodInfo => typeof(PackStructManagedAutomaticExtension).GetMethod("GetPackForType", BindingFlags.NonPublic | BindingFlags.Static);
        internal static Dictionary<Type, IPackagerMethodGeneric> Cash = new Dictionary<Type, IPackagerMethodGeneric>();

        /// <summary>
        /// Tries to create an unpacking method for a managed type
        /// </summary>
        /// <param name="type">Target type</param>
        /// <returns></returns>
        public static IPackagerMethodGeneric Make<T>()
        {
            if (Cash.TryGetValue(typeof(T), out IPackagerMethodGeneric packager))
            {
                return packager;
            }

            if (typeof(T).IsUnManaged() == true)
                throw new Exception("This type is unmanaged!");

            //бух
            MethodInfo mi = GetPackFromType_MethodInfo.MakeGenericMethod(typeof(T));
            IPackagerMethodGeneric pack = (IPackagerMethodGeneric)mi.Invoke(null, null);

            Cash.Add(typeof(T), pack);
            return pack;
        }

        private static IPackagerMethodGeneric GetPackForType<T>()
        {
            Delegate_GetSize<T> action_GetSize;
            Delegate_Action_PackUP<T> action_PackUP;
            Delegate_UnPack<T> action_UnPack;

            if (typeof(T).IsArray)
            {
                MethodInfo mi = PackArray_MethodInfo.MakeGenericMethod(typeof(T), typeof(T).GetElementType());
                delegate_PackGenerator<T> dpa = (delegate_PackGenerator<T>)mi.CreateDelegate(typeof(delegate_PackGenerator<T>));

                dpa.Invoke(out action_GetSize, out action_PackUP, out action_UnPack);
            }
            else
            {
                throw new NotImplementedException();
            }

            return new PackStructManagedAutomatic<T>(action_GetSize, action_PackUP, action_UnPack);
        }

        private static unsafe void PackArray<TArray, TElement>(
            out Delegate_GetSize<TArray> action_GetSize,
            out Delegate_Action_PackUP<TArray> action_PackUP,
            out Delegate_UnPack<TArray> action_UnPack)
        {
            Type elementType = typeof(TElement);
            IPackagerMethod<TElement> pack = (IPackagerMethod<TElement>)Pack.GetMethods<TElement>();

            if (typeof(TArray).GetArrayRank() == 1)
            {
                if (elementType.IsUnManaged())
                {
                    int elementSize = Marshal.SizeOf(elementType);

                    action_GetSize = (TArray source) =>
                    {
                        return (source as TElement[]).Length * elementSize + sizeof(int);
                    };
                }
                else
                {
                    action_GetSize = (TArray source) =>
                    {
                        long size = sizeof(int);
                        TElement[] array = source as TElement[];

                        int length = array.Length;
                        for (int i = 0; i < length; i++)
                        {
                            size += pack.GetSize(array[i]);
                        }

                        return size;
                    };
                }

                action_PackUP = (byte* destination, TArray source) =>
                {
                    TElement[] array = source as TElement[];
                    int length = array.Length;

                    *(int*)destination = length;
                    destination += sizeof(int);

                    long size = sizeof(int);
                    long tmSize;

                    for (int i = 0; i < length; i++)
                    {
                        tmSize = pack.PackUP(destination, array[i]);
                        destination += tmSize;
                        size += tmSize;
                    }

                    return size;
                };

                action_UnPack = (byte* source, out TArray destination) =>
                {
                    int length = *(int*)source;
                    source += sizeof(int);

                    long size = sizeof(int);
                    long tmSize;

                    TElement[] array = new TElement[length];

                    for (int i = 0; i < length; i++)
                    {
                        tmSize = pack.UnPack(source, out array[i]);
                        source += tmSize;
                        size += tmSize;
                    }

                    destination = (TArray)(object)array;

                    return size;
                };

            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// Automatic packer of managed simple types and classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PackStructManagedAutomatic<T> : IPackagerMethod<T>, IPackagerMethodGeneric
    {

        public Type TargetType => typeof(T);

        private readonly Delegate_GetSize<T> Action_GetSize;
        private readonly Delegate_Action_PackUP<T> Action_PackUP;
        private readonly Delegate_UnPack<T> Action_UnPack;

        internal PackStructManagedAutomatic(Delegate_GetSize<T> action_GetSize, Delegate_Action_PackUP<T> action_PackUP, Delegate_UnPack<T> action_UnPack)
        {
            Action_GetSize = action_GetSize;
            Action_PackUP = action_PackUP;
            Action_UnPack = action_UnPack;
        }

        public long GetSize(T source) => Action_GetSize.Invoke(source);

        public unsafe long PackUP(byte* destination, T source) => Action_PackUP.Invoke(destination, source);

        public unsafe long UnPack(byte* source, out T destination) => Action_UnPack.Invoke(source, out destination);
    }
}