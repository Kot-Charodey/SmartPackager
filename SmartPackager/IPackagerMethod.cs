namespace SmartPackager
{
    using ByteStack;
    /// <summary>
    /// Defines the unpacking and packing methods for the selected type
    /// </summary>
    /// <typeparam name="T">target type</typeparam>
    public unsafe interface IPackagerMethod<T> : IPackagerMethodGeneric
    {
        /// <summary>
        /// Packs data into a buffer
        /// </summary>
        /// <param name="writer">packaging interface</param>
        /// <param name="source">packing data</param>
        void PackUP(ref StackWriter writer, T source);
        /// <summary>
        /// Unpacking data from the buffer
        /// </summary>
        /// <param name="reader">unpacking interface</param>
        /// <param name="destination">data to be unpacked</param>
        void UnPack(ref StackReader reader, out T destination);
        /// <summary>
        /// Calculate the required size when packing
        /// </summary>
        /// <param name="meter">buffer length measurement interface</param>
        /// <param name="source">the object for which you need to calculate the size size</param>
        void GetSize(ref StackMeter meter, T source);
    }
}