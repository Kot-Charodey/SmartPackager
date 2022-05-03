using System;

using static SmartPackager.Automatic.PackStructManagedAutomaticExtension;

namespace SmartPackager.Automatic
{
    /// <summary>
    /// Automatic packer of managed classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class PackStructManagedAutomaticHeapArray<T> : PackStructManagedAutomaticHeap<T>
    {
        internal PackStructManagedAutomaticHeapArray(Delegate_GetSizeHeap<T> action_GetSize, Delegate_PackUPHeap<T> action_PackUP, Delegate_UnPackHeap<T> action_UnPack) : base(action_GetSize, action_PackUP, action_UnPack)
        {
        }

        public override Type TargetType => typeof(T);
        public override bool IsFixedSize => false;

        public override int GetSize(T source, ManagedHeap heap)
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
                heap.AllocateHeap(source);
                return sizeof(byte) + Action_GetSize.Invoke(source, heap);
            }
        }

        public override unsafe int PackUP(byte* destination, ManagedHeap heap, T source)
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
                heap.AllocateHeap(source, destination);
                return sizeof(byte) + Action_PackUP.Invoke(destination, heap, source);
            }
        }

        public override unsafe int UnPack(byte* source, ManagedHeap heap, out T destination)
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
                    int s = Action_UnPack.Invoke(source, heap, out destination);
                    heap.AllocateHeap(destination, source);
                    return sizeof(byte) + s;
                default:
                    throw new Exception("Invalid expression for unpacking");
            }
        }

        public override unsafe int PackUP(byte* destination, T source) => PackUP(destination, new ManagedHeap(destination), source);

        public override int GetSize(T source) => GetSize(source, new ManagedHeap());
    }
}