using System;

namespace SmartPackager.BasicPackMethods
{
    internal class PackDateTime : IPackagerMethod<DateTime>
    {
        public Type TargetType => typeof(DateTime);

        public bool IsFixedSize => true;

        public unsafe int PackUP(byte* destination, DateTime source)
        {
            *(long*)destination = source.Ticks;
            return sizeof(long);
        }

        public unsafe int UnPack(byte* source, out DateTime destination)
        {
            DateTime pt = new DateTime(*(long*)source);
            destination = pt;
            return sizeof(long);
        }

        public int GetSize(DateTime source)
        {
            return sizeof(long);
        }
    }
}
