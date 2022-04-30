using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPackager
{
    internal unsafe class ManagedHeap
    {
        public List<KeyValuePair<object, int>> HeapObjects;
        public int HeapOffset = 0;
        public byte* HeapStartPoint;


        public byte* AllocateHeap(object ob,int size)
        {
            byte* ptr = HeapStartPoint + HeapOffset;
            HeapObjects.Add(new KeyValuePair<object, int>(ob, HeapOffset));
            HeapOffset += size;
            return ptr;
        }

        public bool TryGetHeap(object ob,out byte* pos)
        {
            for(int i = 0; i < HeapObjects.Count; i++)
            {
                if (HeapObjects[i].Key == ob)
                {
                    pos = HeapStartPoint + HeapObjects[i].Value;
                    return true;
                }
            }
            pos = (byte*)-1;
            return false;
        }
    }
}
