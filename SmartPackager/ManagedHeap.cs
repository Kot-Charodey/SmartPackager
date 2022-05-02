using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPackager
{
    internal unsafe class ManagedHeap
    {
        private readonly List<KeyValuePair<object, int>> HeapObjects = new List<KeyValuePair<object, int>>();
        private readonly byte* HeapStart;

        public ManagedHeap(byte* heapStart,object crObj)
        {
            HeapStart = heapStart;
            HeapObjects.Add(new KeyValuePair<object, int>(crObj, 0));
        }

        public ManagedHeap(byte* heapStart)
        {
            HeapStart = heapStart;
        }


        public ManagedHeap()
        {
            HeapStart = (byte*)-1;
        }

        public void AllocateHeap(object ob)
        {
            HeapObjects.Add(new KeyValuePair<object, int>(ob, -1));
        }

        public void AllocateHeap(object ob, byte* pos)
        {
            HeapObjects.Add(new KeyValuePair<object, int>(ob, (int)(pos - HeapStart)));
        }

        public void AllocateHeap(object ob, int pos)
        {
            HeapObjects.Add(new KeyValuePair<object, int>(ob, pos));
        }

        public bool TryGetHeap(object ob, out int pos)
        {
            for (int i = 0; i < HeapObjects.Count; i++)
            {
                if (HeapObjects[i].Key == ob)
                {
                    pos = HeapObjects[i].Value;
                    return true;
                }
            }
            pos = -1;
            return false;
        }

        public bool TryGetHeap(int pos, out object ob)
        {
            for (int i = 0; i < HeapObjects.Count; i++)
            {
                if (HeapObjects[i].Value == pos)
                {
                    ob = HeapObjects[i].Key;
                    return true;
                }
            }
            ob = null;
            return false;
        }

        public byte* CalcPoint(int pos)
        {
            return HeapStart + pos;
        }
    }
}
