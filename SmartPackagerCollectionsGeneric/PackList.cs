using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartPackager.Collections.Generic
{
    using SmartPackager;
    using SmartPackager.ByteStack;

    public class PackList<T> : IPackagerMethod<List<T>>
    {
        private static readonly IPackagerMethod<T[]> PackBase = Packager.GetMethod<T[]>();

        public Type TargetType => typeof(List<T>);

        public bool IsFixedSize => false;

        public void GetSize(ref StackMeter meter, List<T> source)
        {
            T[] data = null;
            if (source != null)
                data = source.ToArray();
            PackBase.GetSize(ref meter, data);
        }

        public void PackUP(ref StackWriter writer, List<T> source)
        {
            T[] data = null;
            if (source != null)
                data = source.ToArray();
            PackBase.PackUP(ref writer, data);

        }

        public void UnPack(ref StackReader reader, out List<T> destination)
        {
            PackBase.UnPack(ref reader, out var data);
            if (data != null)
            {
                destination = new List<T>(data);
            }
            else
            {
                destination = null;
            }
        }
    }
}