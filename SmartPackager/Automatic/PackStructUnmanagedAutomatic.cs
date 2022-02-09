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

        public unsafe int PackUP(byte* destination, T source)
        {
            *(T*)destination = source;
            return sizeof(T);
        }

        public unsafe int UnPack(byte* source, out T destination)
        {
            destination = *(T*)source;
            return sizeof(T);
        }

        public unsafe int GetSize(T source)
        {
            return sizeof(T);
        }
    }
}
