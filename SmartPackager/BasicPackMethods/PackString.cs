using System;
using SmartPackager.ByteStack;

namespace SmartPackager.BasicPackMethods
{
    internal class PackString : IPackagerMethod<string>
    {
        public Type TargetType => typeof(string);
        public bool IsFixedSize => false;

        public void PackUP(ref StackWriter writer, string source)
        {
            if (source == null)
            {
                writer.WriteLength(-1);
            }
            else
            {
                writer.WriteLength(source.Length);
                writer.Write(source.ToCharArray());
            }
        }

        public void UnPack(ref StackReader reader, out string destination)
        {
            int len = reader.ReadLength();
            if (len < 0)
            {
                destination = null;
            }
            else
            {
                destination = new string(reader.Read<char>(len));
            }
        }

        public void GetSize(ref StackMeter meter, string source)
        {
            meter.AddLength();
            if (source != null)
            {
                meter.Add<char>(source.Length);
            }
        }
    }
}