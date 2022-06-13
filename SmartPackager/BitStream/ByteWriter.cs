using System.Runtime.CompilerServices;

#if DEBUG
[assembly: InternalsVisibleTo("UnitTest")]
#endif
namespace SmartPackager.BitStream
{
    /// <summary>
    /// Записывает данные в массив
    /// </summary>
    public struct ByteWriter
    {
        private readonly UnsafeArray UnsafeArray;
        private int Pos;

        internal ByteWriter(UnsafeArray unsafeArray)
        {
            UnsafeArray = unsafeArray;
            Pos = 0;
        }
        
        public unsafe void Write<T>(T val) where T : unmanaged
        {
            UnsafeArray.Set(Pos, val);
            Pos += sizeof(T);
        }

        public unsafe void Write<T>(T[] val) where T : unmanaged
        {
            UnsafeArray.Set(Pos, val);
            Pos += sizeof(T) * val.Length;
        }

        public void WriteLength(int length)
        {
            UnsafeArray.Set(Pos, length);
            Pos += sizeof(int);
        }

        public void WriteExists(bool exists)
        {
            UnsafeArray.Set(Pos, exists);
            Pos += sizeof(bool);
        }

        /// <summary>
        /// Создаёт ссылку на текущию позицию
        /// </summary>
        /// <returns>ссылка</returns>
        public ByteRef GetReference()
        {
            return new ByteRef(Pos);
        }

        public void WriteReference(ByteRef byteRef)
        {
            UnsafeArray.Set(Pos, byteRef.GetPoint());
            Pos += sizeof(int);
        }
    }
}