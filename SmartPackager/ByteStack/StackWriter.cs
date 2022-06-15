using System.Runtime.CompilerServices;

#if DEBUG
[assembly: InternalsVisibleTo("UnitTest")]
#endif
namespace SmartPackager.ByteStack
{

    /// <summary>
    /// Записывает данные в массив
    /// </summary>
    public struct StackWriter
    {
        private readonly UnsafeArray UnsafeArray;
        private readonly RefArray RefArray;
        private int Pos;

        internal StackWriter(UnsafeArray unsafeArray)
        {
            UnsafeArray = unsafeArray;
            RefArray = new RefArray();
            Pos = 0;
        }

        /// <summary>
        /// Записать значение
        /// </summary>
        /// <typeparam name="T">тип значения</typeparam>
        /// <param name="val">значение</param>
        public unsafe void Write<T>(T val) where T : unmanaged
        {
            UnsafeArray.Set(Pos, val);
            Pos += sizeof(T);
        }

        /// <summary>
        /// Записать неуправляймый массив
        /// </summary>
        /// <typeparam name="T">тип массива</typeparam>
        /// <param name="val">массив</param>
        public unsafe void Write<T>(T[] val) where T : unmanaged
        {
            UnsafeArray.Set(Pos, val);
            Pos += sizeof(T) * val.Length;
        }

        /// <summary>
        /// Записать длинну чего-либо(число)
        /// </summary>
        /// <param name="length">число</param>
        public void WriteLength(int length)
        {
            UnsafeArray.Set(Pos, length);
            Pos += sizeof(int);
        }

        /// <summary>
        /// Создаёт ссылку на объект
        /// </summary>
        /// <returns>вернёт true если данный объект упаковывается в первые и не null (иначе упаковывать не надо)</returns>
        public bool MakeReference(object val)
        {
            if (val == null)
            {
                UnsafeArray.Set(Pos, RefPoint.NULL);
                Pos += sizeof(int);
                return false;
            }
            else
            {
                if (RefArray.Exists(val, out var point))
                {
                    UnsafeArray.Set(Pos, point.Point);
                    Pos += sizeof(int);
                    return false;
                }
                else
                {
                    UnsafeArray.Set(Pos, RefPoint.DATA);
                    Pos += sizeof(int);
                    RefArray.AddRef(new RefPoint(Pos, val));
                    return true;
                }
            }
        }
    }
}