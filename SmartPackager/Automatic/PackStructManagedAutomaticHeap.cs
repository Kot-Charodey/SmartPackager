using System;

using static SmartPackager.Automatic.PackStructManagedAutomaticExtension;

namespace SmartPackager.Automatic
{
    /// <summary>
    /// Automatic packer of managed classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class PackStructManagedAutomaticHeap<T> : IPackagerMethod<T>
    {
        public Type TargetType => typeof(T);
        public bool IsFixedSize => Data.IsFixedSize;

        private readonly MethodsDataHeap<T> Data;

        internal PackStructManagedAutomaticHeap(MethodsDataHeap<T> methods)
        {
            Data = methods;
        }

        public int GetSize(T source, ManagedHeap heap)
        {
            if (source == null)
            {
                return sizeof(byte);
            }
            else if (heap.TryGetHeap(source, out _))
            {
                return sizeof(byte) + sizeof(int);
            }
            else
            {
                return sizeof(byte) + Data.Action_GetSize.Invoke(source, heap);
            }
        }

        public unsafe int PackUP(byte* destination, ManagedHeap heap, T source)
        {
            if (source == null)
            {
                *destination = (byte)PointerType.Null;
                return sizeof(byte);
            }
            else if (heap.TryGetHeap(source, out int pos))
            {
                *destination = (byte)PointerType.Pointer;
                destination++;
                *(int*)destination = pos;
                return sizeof(byte) + sizeof(int);
            }
            else
            {
                *destination = (byte)PointerType.Data;
                destination++;
                return sizeof(byte) + Data.Action_PackUP.Invoke(destination, heap, source);
            }
        }

        public unsafe int UnPack(byte* source, ManagedHeap heap, out T destination)
        {
            PointerType pp = (PointerType)(*source);
            source++;
            switch (pp)
            {
                case PointerType.Null:
                    destination = default;
                    return sizeof(byte);
                case PointerType.Pointer:
                    int pos = *(int*)source;
                    source += sizeof(int);
                    if (heap.TryGetHeap(pos, out var ob))
                    {
                        destination = (T)ob;
                        return sizeof(byte) + sizeof(int);
                    }
                    else
                        throw new Exception("Invalid expression for unpacking");
                case PointerType.Data:
                    int s = Data.Action_UnPack.Invoke(source, heap, out destination);
                    return sizeof(byte) + s;
                default:
                    throw new Exception("Invalid expression for unpacking");
            }
        }

        public virtual unsafe int GetSize(T source) => GetSize(source, new ManagedHeap());

        public virtual unsafe int PackUP(byte* destination, T source) => PackUP(destination, new ManagedHeap(destination), source);

        public unsafe int UnPack(byte* source, out T destination) => UnPack(source, new ManagedHeap(source), out destination);
    }
}