﻿using System;

using static SmartPackager.Automatic.PackStructManagedAutomaticExtension;

namespace SmartPackager.Automatic
{
    /// <summary>
    /// Automatic packer of managed classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class PackStructManagedAutomaticHeap<T> : IPackagerMethod<T>
    {
        public virtual Type TargetType => typeof(T);
        public virtual bool IsFixedSize => false;

        protected readonly Delegate_GetSizeHeap<T> Action_GetSize;
        protected readonly Delegate_PackUPHeap<T> Action_PackUP;
        protected readonly Delegate_UnPackHeap<T> Action_UnPack;


        internal PackStructManagedAutomaticHeap(Delegate_GetSizeHeap<T> action_GetSize, Delegate_PackUPHeap<T> action_PackUP, Delegate_UnPackHeap<T> action_UnPack)
        {
            Action_GetSize = action_GetSize;
            Action_PackUP = action_PackUP;
            Action_UnPack = action_UnPack;
        }

        public virtual int GetSize(T source, ManagedHeap heap) => Action_GetSize.Invoke(source, heap);

        public virtual unsafe int PackUP(byte* destination, ManagedHeap heap, T source) => Action_PackUP.Invoke(destination, heap, source);

        public virtual unsafe int UnPack(byte* source, ManagedHeap heap, out T destination) => Action_UnPack.Invoke(source, heap, out destination);

        public virtual unsafe int GetSize(T source) => GetSize(source, new ManagedHeap((byte*)0, source));

        public virtual unsafe int PackUP(byte* destination, T source) => PackUP(destination, new ManagedHeap(destination, source), source);

        public unsafe int UnPack(byte* source, out T destination) => UnPack(source, new ManagedHeap(source), out destination);
    }
}