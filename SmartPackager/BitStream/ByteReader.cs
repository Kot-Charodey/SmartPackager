namespace SmartPackager.BitStream
{
    /// <summary>
    /// Считывает данные из буфера
    /// </summary>
    public struct ByteReader
    {
        private readonly UnsafeArray UnsafeArray;
        private int Pos;

        private ByteReader(ByteReader reader, int pos)
        {
            UnsafeArray = reader.UnsafeArray;
            Pos = pos;
        }

        internal ByteReader(UnsafeArray unsafeArray)
        {
            UnsafeArray = unsafeArray;
            Pos = 0;
        }

        /// <summary>
        /// Считать значение из буфера
        /// </summary>
        /// <typeparam name="T">тип значения</typeparam>
        /// <returns>считаное значение</returns>
        public unsafe T Read<T>() where T : unmanaged
        {
            var data = UnsafeArray.Get<T>(Pos);
            Pos += sizeof(T);
            return data;
        }

        public unsafe T[] Read<T>(int length) where T : unmanaged
        {
            var data = UnsafeArray.Get<T>(Pos, length);
            Pos += sizeof(T);
            return data;
        }

        public int ReadLength()
        {
            var len = UnsafeArray.Get<int>(Pos);
            Pos += sizeof(int);
            return len;
        }

        public bool ReadExists()
        {
            var exists = UnsafeArray.Get<bool>(Pos);
            Pos += sizeof(bool);
            return exists;
        }

        public ByteRef ReadReference()
        {
            ByteRef byteRef = new ByteRef(UnsafeArray.Get<int>(Pos));
            Pos += sizeof(int);
            return byteRef;
        }

        /*
        /// <summary>
        /// Осуществляет переход по ссылке
        /// </summary>
        /// <param name="byteRef">ссылка на регион памяти</param>
        /// <returns>создаёт ByteReader начинающий чтение текущего буфера с региона куда ссылается сылка</returns>
        public ByteReader GotoReference(ByteRef byteRef)
        {
            return new ByteReader(this, byteRef.GetPoint());
        }
        */
    }
}