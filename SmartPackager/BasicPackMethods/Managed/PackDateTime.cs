using System;

namespace SmartPackager.BasicPackMethods.Managed
{
    public class PackDateTime : IPackagerMethod
    {
        public Type TargetType => typeof(DateTime);

        public unsafe long PackUP(byte* destination, object source)
        {
            *(long*)destination = ((DateTime)source).Ticks;
            return sizeof(long);
        }

        public unsafe long UnPack(byte* source, out object destination)
        {
            DateTime pt = new DateTime(*(long*)source);
            destination = pt;
            return sizeof(long);
        }

        public long GetSize(object source)
        {
            return sizeof(long);
        }
    }
}
