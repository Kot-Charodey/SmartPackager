using System;

namespace SmartPackager
{
    public class Packager
    {
        private Pack Pack;

        public Packager(Pack types)
        {
            Pack = types;
        }

        public long CalcNeedSize<T1>(T1 t1)
        {
            return ((IPackagerMethod<T1>)Pack.PackFunction[0]).GetSize(t1);
        }

        public unsafe void PackUP<T1>(byte[] destination, long offset, T1 t1)
        {
            fixed (byte* dest = &destination[offset])
            {
                byte* point = dest;
                ((IPackagerMethod<T1>)Pack.PackFunction[0]).PackUP(point, t1);
            }
        }

        public unsafe void UnPack<T1>(byte[] source, long offset, out T1 t1)
        {
            fixed (byte* dest = &source[offset])
            {
                byte* point = dest;
                ((IPackagerMethod<T1>)Pack.PackFunction[0]).UnPack(point, out t1);
            }
        }
    }
}