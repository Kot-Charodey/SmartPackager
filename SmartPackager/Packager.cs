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

        private void CheckArrayLength(object[] data)
        {
            if (data.Length != Pack.PackFunction.Length)
            {
                throw new ArgumentOutOfRangeException("Need length " + Pack.PackFunction.Length);
            }
        }

        public long CalcNeedSize(object[] data)
        {
            CheckArrayLength(data);
            long size = 0;
            for(int i = 0; i < data.Length; i++)
            {
                size += Pack.PackFunction[i].GetSize(data[i]);
            }
            return size;
        }

        public unsafe void PackUP(byte[] destination, long offset, object[] data)
        {
            CheckArrayLength(data);
            fixed (byte* dest = &destination[offset]){
                byte* point = dest;
                for (int i = 0; i < data.Length; i++)
                {
                    point += Pack.PackFunction[i].PackUP(point, data[i]);
                }
            }
        }

        public unsafe object[] UnPack(byte[] source, long offset)
        {
            object[] data = new object[Pack.PackFunction.Length];
            fixed (byte* dest = &source[offset])
            {
                byte* point = dest;
                for (int i = 0; i < data.Length; i++)
                {
                    point += Pack.PackFunction[i].UnPack(point,out data[i]);
                }
            }
            return data; 
        }
    }
}
