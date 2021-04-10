using System;

namespace SmartPackager.BasicPackMethods.Managed
{
    public class PackString : IPackagerMethod
    {
        public Type TargetType => typeof(string);

        public unsafe long PackUP(byte* destination, object source)
        {
            string data = (string)source;
            *(int*)destination = data.Length;
            destination += sizeof(int);

            for (int i = 0; i < data.Length; i++)
            {
                *(char*)destination = data[i];
                destination += sizeof(char);
            }

            return data.Length * sizeof(char) + sizeof(int);
        }

        public unsafe long UnPack(byte* source, out object destination)
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

        public long GetSize(object source)
        {
            return ((string)source).Length * sizeof(char) + sizeof(int);
        }
    }
}
