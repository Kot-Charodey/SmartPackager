using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartPackager.Collections.Generic
{
    using SmartPackager;
    using SmartPackager.ByteStack;

    public class PackDictionary<Key, Value> : IPackagerMethod<Dictionary<Key, Value>>
    {
        private static readonly IPackagerMethod<(Key, Value)[]> PackBase = Packager.GetMethod<(Key, Value)[]>();

        public Type TargetType => typeof(Dictionary<Key, Value>);

        public bool IsFixedSize => false;

        public void GetSize(ref StackMeter meter, Dictionary<Key, Value> source)
        {
            (Key, Value)[] data = null;
            if (source != null)
                data = (from item in source select (item.Key, item.Value)).ToArray();
            PackBase.GetSize(ref meter, data);
        }

        public void PackUP(ref StackWriter writer, Dictionary<Key, Value> source)
        {
            (Key, Value)[] data = null;
            if (source != null)
                data = (from item in source select (item.Key, item.Value)).ToArray();
            PackBase.PackUP(ref writer, data);

        }

        public void UnPack(ref StackReader reader, out Dictionary<Key, Value> destination)
        {
            PackBase.UnPack(ref reader, out var data);
            if (data != null)
            {
                destination = data.ToDictionary(x => x.Item1, x => x.Item2);
            }
            else
            {
                destination = null;
            }
        }
    }
}