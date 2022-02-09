using System;

using static SmartPackager.Automatic.PackStructManagedAutomaticExtension;

namespace SmartPackager.Automatic
{
    /// <summary>
    /// Automatic packer of managed simple types and classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class PackStructManagedAutomatic<T> : IPackagerMethod<T>
    {
        public Type TargetType => typeof(T);

        public bool IsFixedSize { get; private set; }

        private readonly Delegate_GetSize<T> Action_GetSize;
        private readonly Delegate_PackUP<T> Action_PackUP;
        private readonly Delegate_UnPack<T> Action_UnPack;

        internal PackStructManagedAutomatic(Delegate_GetSize<T> action_GetSize, Delegate_PackUP<T> action_PackUP, Delegate_UnPack<T> action_UnPack, bool isFixedSize)
        {
            Action_GetSize = action_GetSize;
            Action_PackUP = action_PackUP;
            Action_UnPack = action_UnPack;
            IsFixedSize = isFixedSize;
        }

        public int GetSize(T source) => Action_GetSize.Invoke(source);

        public unsafe int PackUP(byte* destination, T source) => Action_PackUP.Invoke(destination, source);

        public unsafe int UnPack(byte* source, out T destination) => Action_UnPack.Invoke(source, out destination);
    }
}