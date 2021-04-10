using System;

namespace SmartPackager.BasicPackMethods.Managed
{
    public class PackDateTime : IPackagerMethod<DateTime>, IPackagerMethodGeneric
    {
        public Type TargetType => typeof(DateTime);

        public unsafe long PackUP(byte* destination, DateTime source)
        {
            *(long*)destination = source.Ticks;
            return sizeof(long);
        }

        public unsafe long UnPack(byte* source, out DateTime destination)
        {
            DateTime pt = new DateTime(*(long*)source);
            destination = pt;
            return sizeof(long);
        }

        public long GetSize(DateTime source)
        {
            return sizeof(long);
        }
    }
}
