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
        private delegate void delegate_PackGenerator(out Delegate_GetSize action_GetSize, out Delegate_Action_PackUP action_PackUP, out Delegate_UnPack action_UnPack);

        internal unsafe delegate long Delegate_GetSize(object source);
        internal unsafe delegate long Delegate_Action_PackUP(byte* destination, object sourcen);
        internal unsafe delegate long Delegate_UnPack(byte* source, out object destination);

        private static MethodInfo GetMethods_MethodInfo => typeof(Pack).GetMethod("GetMethods");
        private static MethodInfo PackArray_MethodInfo => typeof(PackStructManagedAutomaticExtension).GetMethod("PackArray", BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo GetPackFromType_MethodInfo => typeof(PackStructManagedAutomaticExtension).GetMethod("GetPackFromType", BindingFlags.NonPublic | BindingFlags.Static);
        internal static Dictionary<Type, IPackagerMethod> Cash = new Dictionary<Type, IPackagerMethod>();

        /// <summary>
        /// Tries to create an unpacking method for a managed type
        /// </summary>
        /// <param name="type">Target type</param>
        /// <returns></returns>
        public static IPackagerMethod Make(Type type)
        {
            if (Cash.TryGetValue(type, out IPackagerMethod packager))
            {
                return packager;
            }

            if (type.IsUnManaged() == true)
                throw new Exception("This type is unmanaged!");


            MethodInfo mi = GetPackFromType_MethodInfo.MakeGenericMethod(type);
            IPackagerMethod pack = (IPackagerMethod)mi.Invoke(null, null);

            Cash.Add(type, pack);
            return pack;
        }

        private static IPackagerMethod GetPackFromType<T>()
        {
            Delegate_GetSize action_GetSize;
            Delegate_Action_PackUP action_PackUP;
            Delegate_UnPack action_UnPack;

            if (typeof(T).IsArray)
            {
                MethodInfo mi = PackArray_MethodInfo.MakeGenericMethod(typeof(T), typeof(T).GetElementType());
                delegate_PackGenerator dpa = (delegate_PackGenerator)mi.CreateDelegate(typeof(delegate_PackGenerator));

                dpa.Invoke(out action_GetSize, out action_PackUP, out action_UnPack);
            }
            else
            {
                throw new NotImplementedException();
            }

            return new PackStructManagedAutomatic<T>(action_GetSize, action_PackUP, action_UnPack);
        }

        private static unsafe void PackArray<TArray, TElement>(
            out Delegate_GetSize action_GetSize,
            out Delegate_Action_PackUP action_PackUP,
            out Delegate_UnPack action_UnPack)
        {
            Type elementType = typeof(TElement);
            IPackagerMethod pack = Pack.GetMethods<TElement>();

            if (typeof(TArray).GetArrayRank() == 1)
            {
                if (elementType.IsUnManaged())
                {
                    int elementSize = Marshal.SizeOf(elementType);

                    action_GetSize = (object source) =>
                    {
                        return ((TElement[])source).Length * elementSize + sizeof(int);
                    };
                }
                else
                {
                    action_GetSize = (object source) =>
                    {
                        long size = sizeof(int);
                        TElement[] array = (TElement[])source;

                        int length = array.Length;
                        for (int i = 0; i < length; i++)
                        {
                            size += pack.GetSize(array[i]);
                        }

                        return size;
                    };
                }

                action_PackUP = (byte* destination, object source) =>
                {
                    TElement[] array = (TElement[])source;
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

                action_UnPack = (byte* source, out object destination) =>
                {
                    int length = *(int*)source;
                    source += sizeof(int);

                    long size = sizeof(int);
                    long tmSize;

                    TElement[] array = new TElement[length];

                    for (int i = 0; i < length; i++)
                    {
                        tmSize = pack.UnPack(source, out object tmData);
                        array[i] = (TElement)tmData;
                        source += tmSize;
                        size += tmSize;
                    }

                    destination = array;

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
    public class PackStructManagedAutomatic<T> : IPackagerMethod
    {

        public Type TargetType => typeof(T);

        private readonly Delegate_GetSize Action_GetSize;
        private readonly Delegate_Action_PackUP Action_PackUP;
        private readonly Delegate_UnPack Action_UnPack;

        internal PackStructManagedAutomatic(Delegate_GetSize action_GetSize, Delegate_Action_PackUP action_PackUP, Delegate_UnPack action_UnPack)
        {
            Action_GetSize = action_GetSize;
            Action_PackUP = action_PackUP;
            Action_UnPack = action_UnPack;
        }

        public long GetSize(object source) => Action_GetSize.Invoke(source);

        public unsafe long PackUP(byte* destination, object source) => Action_PackUP.Invoke(destination, source);

        public unsafe long UnPack(byte* source, out object destination) => Action_UnPack.Invoke(source, out destination);
    }
}