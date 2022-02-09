using System;

namespace SmartPackager.BasicPackMethods
{
    /// <summary>
    /// Description of the storage of packaged data:
    /// if not null-int[array length];char[letters]
    /// if null-int[array length]=-1
    /// </summary>
    internal class PackString : IPackagerMethod<string>
    {
        public Type TargetType => typeof(string);

        public bool IsFixedSize => false;

        public unsafe int PackUP(byte* destination, string source)
        {
            if (source == null)
            {
                *(int*)destination = -1;
                return sizeof(int);
            }
            else
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
        }

        public unsafe int UnPack(byte* source, out string destination)
        {
            if (*(int*)source == -1)
            {
                destination = null;
                return sizeof(int);
            }
            else
            {
                int size = *(int*)source;
                source += sizeof(int);
                char[] data = new char[size];

                for (int i = 0; i < size; i++)
                {
                    data[i] = *(char*)source;
                    source += sizeof(char);
                }

                destination = new string(data);
                return size * sizeof(char) + sizeof(int);
            }
        }

        public int GetSize(string source)
        {
            if (source == null)
                return sizeof(int);
            else
                return source.Length * sizeof(char) + sizeof(int);
        }
    }
}