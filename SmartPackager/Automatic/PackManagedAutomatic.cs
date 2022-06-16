using SmartPackager.ByteStack;
using System;

namespace SmartPackager.Automatic
{
    /// <summary>
    /// Используется автоматическим упаковщиком
    /// </summary>
    internal class PackManagedAutomatic<T> : IPackagerMethod<T>
    {
        public Type TargetType { get; set; }
        public bool IsFixedSize { get; set; }

        public GenericGetSize<T> MembersGetSize;
        public GenericPackUP<T> MembersPackUp;
        public GenericUnPack<T> MembersUnPack;


        public GenericGetSizeMain<T> MainGetSize;
        public GenericPackUPMain<T> MainPackUp;
        public GenericUnPackMain<T> MainUnPack;


        public void GetSize(ref StackMeter meter, T source) => MainGetSize(ref meter, source, MembersGetSize);

        public void PackUP(ref StackWriter writer, T source) => MainPackUp(ref writer, source, MembersPackUp);

        public void UnPack(ref StackReader reader, out T destination) => MainUnPack(ref reader, out destination, MembersUnPack);
    }
}