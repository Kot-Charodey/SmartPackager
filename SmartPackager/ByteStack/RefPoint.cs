namespace SmartPackager.ByteStack
{
    /// <summary>
    /// Ссылка но объект (если Point>=0 то ссылка инача NULL или DATA)
    /// </summary>
    internal struct RefPoint
    {
        public const int NULL = -1;
        public const int DATA = -2;

        public int Point;
        public object Data;

        public RefPoint(int point, object data)
        {
            Point = point;
            Data = data;
        }
    }
}
