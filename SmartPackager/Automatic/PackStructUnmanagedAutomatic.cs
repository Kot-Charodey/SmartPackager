using SmartPackager.ByteStack;
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

        public void PackUP(ref ByteWriter writer, T source)
        {
            writer.Write(source);
        }

        public void UnPack(ref ByteReader reader, out T destination)
        {
            destination = reader.Read<T>();
        }

        public void GetSize(ref ByteMeter meter, T source)
        {
            meter.Add<T>();
        }
    }
}
