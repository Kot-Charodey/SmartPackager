using System.Runtime.CompilerServices;

#if DEBUG
[assembly: InternalsVisibleTo("UnitTest")]
#endif
namespace SmartPackager.ByteStack
{

    /// <summary>
    /// Записывает данные в массив
    /// </summary>
    public struct ByteWriter
    {
        private readonly UnsafeArray UnsafeArray;
        private readonly RefArray RefArray;
        private int Pos;

        internal ByteWriter(UnsafeArray unsafeArray)
        {
            UnsafeArray = unsafeArray;
            RefArray = new RefArray();
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