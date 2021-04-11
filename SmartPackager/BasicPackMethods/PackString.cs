using System;

namespace SmartPackager.BasicPackMethods
{
    public class PackString : IPackagerMethod<string>
    {
        public Type TargetType => typeof(string);

        public bool IsFixedSize => false;

        public unsafe long PackUP(byte* destination, string source)
        {
            *(int*)destination = source.Length;
            destination += sizeof(int);

            for (int i = 0; i < source.Length; i++)
            {
                *(char*)destination = source[i];
                destination += sizeof(char);
            }

            return source.Length * sizeof(char) + sizeof(int);
        }

        public unsafe long UnPack(byte* source, out string destination)
        {
            int size = *(int*)source;
            source += sizeof(int);
            char[] data= new char[size];

            for(int i = 0; i < size; i++)
            {
                data[i] = *(char*)source;
                source += sizeof(char);
            }

            destination = new string(data);
            return size * sizeof(char) + sizeof(int);
        }

        public long GetSize(string source)
        {
            return source.Length * sizeof(char) + sizeof(int);
        }
    }
}
