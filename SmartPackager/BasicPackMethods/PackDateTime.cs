using System;
using SmartPackager.ByteStack;

namespace SmartPackager.BasicPackMethods
{
    internal class PackDateTime : IPackagerMethod<DateTime>
    {
        public Type TargetType => typeof(DateTime);

        public bool IsFixedSize => true;

        public void PackUP(ref ByteWriter writer, DateTime source)
        {
            writer.Write(source.Ticks);
        }

        public void UnPack(ref ByteReader reader, out DateTime destination)
        {
            destination = new DateTime(reader.Read<long>());
        }

        public void GetSize(ref ByteMeter meter, DateTime source)
        {
            meter.Add<long>();
        }
    }
}
