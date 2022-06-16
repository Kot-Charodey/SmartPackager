using System;
using System.Reflection;

namespace SmartPackager.Automatic
{
    internal static class MethodUtil
    {
        public const BindingFlags BindingFind = BindingFlags.NonPublic | BindingFlags.Static;

        public static T MakeGenericDelegate<T>(this MethodInfo method) where T: Delegate
        {
            return MakeGenericDelegate<T>(method, typeof(T).GenericTypeArguments);
        }

        public static T MakeGenericDelegate<T>(this MethodInfo method, params Type[] typeArguments) where T : Delegate
        {
            return (T)method.MakeGenericMethod(typeArguments).CreateDelegate(typeof(T));
        }
    }
}
