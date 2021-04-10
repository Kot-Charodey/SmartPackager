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
        internal unsafe delegate long Delegate_GetSize(object source);
        internal unsafe delegate long Delegate_Action_PackUP(byte* destination, object sourcen);
        internal unsafe delegate long Delegate_UnPack(byte* source, out object destination);

        private static MethodInfo GetMethods_MethodInfo => typeof(Pack).GetMethod("GetMethods");
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

        private static unsafe IPackagerMethod GetPackFromType<T>()
        {
            Delegate_GetSize action_GetSize;
            Delegate_Action_PackUP action_PackUP;
            Delegate_UnPack action_UnPack;

            if (typeof(T).IsArray)
            {
                Type elementType = typeof(T).GetElementType();

                MethodInfo mi = GetMethods_MethodInfo.MakeGenericMethod(elementType);
                IPackagerMethod pack = (IPackagerMethod)mi.Invoke(null, null);

                if (typeof(T).GetArrayRank() == 1)
                {
                    if (elementType.IsUnManaged())
                    {
                        int elementSize = Marshal.SizeOf(elementType);

                        action_GetSize = (object source) =>
                        {
                            return ((Array)source).Length * elementSize + sizeof(int);
                        };
                    }
                    else
                    {
                        action_GetSize = (object source) =>
                        {
                            long size = sizeof(int);

                            int length = ((Array)source).Length;
                            for (int i = 0; i < length; i++)
                            {
                                size += pack.GetSize(((Array)source).GetValue(i));
                            }

                            return size;
                        };
                    }

                    action_PackUP = (byte* destination, object source) =>
                    {
                        int length = ((Array)source).Length;

                        *(int*)destination = length;
                        destination += sizeof(int);

                        long size = sizeof(int);
                        long tmSize;

                        for (int i = 0; i < length; i++)
                        {
                            tmSize = pack.PackUP(destination, ((Array)source).GetValue(i));
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

                        Array array = Array.CreateInstance(elementType, length);

                        for (int i = 0; i < length; i++)
                        {
                            tmSize = pack.UnPack(source, out object tmData);
                            array.SetValue(tmData, i);
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
            else
            {
                throw new NotImplementedException();
            }

            return new PackStructManagedAutomatic<T>(action_GetSize, action_PackUP, action_UnPack);
        }
    }

    /// <summary>
    /// Automatic packer of managed simple types and classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PackStructManagedAutomatic<T> : IPackagerMethod
    {

        public Type TargetType => typeof(T);

        private Delegate_GetSize Action_GetSize;
        private Delegate_Action_PackUP Action_PackUP;
        private Delegate_UnPack Action_UnPack;

        internal PackStructManagedAutomatic(Delegate_GetSize action_GetSize, Delegate_Action_PackUP action_PackUP, Delegate_UnPack action_UnPack)
        {
            Action_GetSize = action_GetSize;
            Action_PackUP = action_PackUP;
            Action_UnPack = action_UnPack;
        }

        public long GetSize(object source) => Action_GetSize.Invoke(source);

        public unsafe long PackUP(byte* destination, object source) => Action_PackUP.Invoke(destination, source);

        public unsafe long UnPack(byte* source, out object destination) => Action_UnPack.Invoke(source,out destination);
    }
}
