using SmartPackager.ByteStack;
using System;

namespace SmartPackager.Automatic
{
    internal class PackUnmanagedAutomatic<T> : IPackagerMethod<T> where T : unmanaged
    {
        public Type TargetType => typeof(T);

        public bool IsFixedSize => true;

        public void PackUP(ref StackWriter writer, T source)
        {
            writer.Write(source);
        }

        public void UnPack(ref StackReader reader, out T destination)
        {
            destination = reader.Read<T>();
        }

        public void GetSize(ref StackMeter meter, T source)
        {
            meter.Add<T>();
        }
    }
}
