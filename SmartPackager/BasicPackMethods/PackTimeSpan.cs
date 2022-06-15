using SmartPackager.ByteStack;
using System;

namespace SmartPackager.BasicPackMethods
{
    internal class PackTimeSpan : IPackagerMethod<TimeSpan>
    {
        public Type TargetType => typeof(TimeSpan);

        public bool IsFixedSize => true;

        public void PackUP(ref StackWriter writer, TimeSpan source)
        {
            writer.Write(source.Ticks);
        }

        public void UnPack(ref StackReader reader, out TimeSpan destination)
        {
            destination = new TimeSpan(reader.Read<long>());
        }

        public void GetSize(ref StackMeter meter, TimeSpan source)
        {
            meter.Add<long>();
        }
    }
}
