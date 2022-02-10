using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPackager.Collections.Generic
{
    using SmartPackager;

    public class PackDictionary<Key, Value> : IPackagerMethod<Dictionary<Key, Value>>
    {
        private static Packager.M<Key[], Value[]> PackBase = Packager.Create<Key[], Value[]>();

        public Type TargetType => typeof(Dictionary<Key, Value>);

        public bool IsFixedSize => false;

        public int GetSize(Dictionary<Key, Value> source)
        {
            return PackBase.CalcNeedSize(source.Keys.ToArray(), source.Values.ToArray());
        }

        public unsafe int PackUP(byte* destination, Dictionary<Key, Value> source)
        {
            return PackBase.PackUP(destination, source.Keys.ToArray(), source.Values.ToArray());
        }

        public unsafe int UnPack(byte* source, out Dictionary<Key, Value> destination)
        {
            int size = PackBase.UnPack(source, out var key, out var value);

            destination = new Dictionary<Key, Value>(key.Length);
            for (int i = 0; i < key.Length; i++)
            {
                destination.Add(key[i], value[i]);
            }
            return size;
        }
    }
}