namespace SmartPackager.ByteStack
{
    /// <summary>
    /// Расчитывает размер данных, которые необходимо упаковать
    /// </summary>
    public struct StackMeter
    {
        private int Length;
        private readonly RefArray RefArray;

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
        /// Добавляет размер который был посчитан зарание
        /// </summary>
        public void AddFixedSize(int size)
        {
            Length += size;
        }

        /// <summary>
        /// Создаёт ссылку на объект
        /// </summary>
        /// <returns>вернёт true если данный объект упаковывается в первые и не null (иначе упаковывать не надо)</returns>
        public bool MakeReference(object val)
        {
            Length += sizeof(int);
            if (val == null)
            {
                return false;
            }
            else
            {
                if (RefArray.Exists(val, out var point))
                {
                    return false;
                }
                else
                {
                    RefArray.AddRef(new RefPoint(Length, val));
                    return true;
                }
            }
        }
    }
}