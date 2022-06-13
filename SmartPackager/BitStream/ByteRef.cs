using System.Runtime.CompilerServices;

#if DEBUG
[assembly: InternalsVisibleTo("UnitTest")]
#endif
namespace SmartPackager.BitStream
{
    /// <summary>
    /// Ссылка на регион памяти
    /// </summary>
    public struct ByteRef
    {
        private readonly int Point;

        internal ByteRef(int point)
        {
            Point = point;
        }

        internal int GetPoint()
        {
            return Point;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator == (ByteRef a,ByteRef b)
        {
            return a.Point == b.Point;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(ByteRef a, ByteRef b)
        {
            return a.Point != b.Point;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is ByteRef @ref &&
                   Point == @ref.Point;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return -1396796455 + Point.GetHashCode();
        }
    }
}