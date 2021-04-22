using System;

namespace SmartPackager.Automatic
{
    internal class PackStructUnmanagedAutomatic<T> : IPackagerMethod<T> where T : unmanaged
    {
        public Type TargetType => typeof(T);

        public bool IsFixedSize => true;

        internal PackStructUnmanagedAutomatic()
        {

        }

        public unsafe long PackUP(byte* destination, T source)
        {
            *(T*)destination = source;
            return sizeof(T);
        }

        public unsafe long UnPack(byte* source, out T destination)
        {
            destination = *(T*)source;
            return sizeof(T);
        }

        public unsafe long GetSize(T source)
        {
            return sizeof(T);
        }
    }
}
