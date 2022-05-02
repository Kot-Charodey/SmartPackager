namespace SmartPackager.Automatic
{
    /// <summary>
    /// object reference
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class RefObject<T>
    {
        public T Object;

        public RefObject()
        {
        }

        public RefObject(T @object)
        {
            Object = @object;
        }
    }
}
