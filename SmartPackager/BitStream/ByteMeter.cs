namespace SmartPackager.BitStream
{
    /// <summary>
    /// Расчитывает размер данных, которые необходимо упаковать
    /// </summary>
    public struct ByteMeter
    {
        private int Length;

        /// <summary>
        /// Получить расчитанный размер
        /// </summary>
        /// <returns>размер необходимый для упаковки данных</returns>
        public int GetCalcLength() => Length;

        /// <summary>
        /// Добавить в расчёт указанный неуправляймый тип
        /// </summary>
        /// <typeparam name="T">тип для которого будет расчитан размер</typeparam>
        public unsafe void Add<T>() where T : unmanaged
        {
            Length += sizeof(T);
        }

        /// <summary>
        /// Добавить в расчёт указанный массив неуправляймого типа
        /// </summary>
        /// <typeparam name="T">тип для которого будет расчитан размер</typeparam>
        /// <param name="size">размер массива (кол-во элементов)</param>
        public unsafe void Add<T>(int size) where T : unmanaged
        {
            Length += sizeof(T) * size;
        }

        /// <summary>
        /// Добавить в расчёт число длинны
        /// </summary>
        public void AddLength()
        {
            Length += sizeof(int);
        }

        /// <summary>
        /// Добавить в расчёт флаг указывающий на существования чего либо
        /// </summary>
        public void AddExists()
        {
            Length += sizeof(bool);
        }

        /// <summary>
        /// Добавить в расчёт ссылку
        /// </summary>
        public ByteRef MakeReference()
        {
            ByteRef @ref= new ByteRef(Length);
            Length += sizeof(int);
            return @ref;
        }
    }
}