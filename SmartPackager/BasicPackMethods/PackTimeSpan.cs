using SmartPackager.ByteStack;
using System;

namespace SmartPackager.BasicPackMethods
{
    internal class PackTimeSpan : IPackagerMethod<TimeSpan>
    {
        public Type TargetType => typeof(TimeSpan);

        public bool IsFixedSize => true;

        public void PackUP(ref ByteWriter writer, TimeSpan source)
        {
            writer.Write(source.Ticks);
        }

        public void UnPack(ref ByteReader reader, out TimeSpan destination)
        {
            destination = new TimeSpan(reader.Read<long>());
        }

        public void GetSize(ref ByteMeter meter, TimeSpan source)
        {
            meter.Add<long>();
        }
    }
}
