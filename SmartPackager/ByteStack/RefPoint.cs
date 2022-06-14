namespace SmartPackager.ByteStack
{
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
