using System;
using SmartPackager.ByteStack;

namespace SmartPackager.BasicPackMethods
{
    internal class PackDateTime : IPackagerMethod<DateTime>
    {
        public Type TargetType => typeof(DateTime);

        public bool IsFixedSize => true;

        public void PackUP(ref StackWriter writer, DateTime source)
        {
            writer.Write(source.Ticks);
        }

        public void UnPack(ref StackReader reader, out DateTime destination)
        {
            destination = new DateTime(reader.Read<long>());
        }

        public void GetSize(ref StackMeter meter, DateTime source)
        {
            meter.Add<long>();
        }
    }
}
