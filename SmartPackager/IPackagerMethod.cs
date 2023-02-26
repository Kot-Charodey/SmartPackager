namespace SmartPackager
{
    using ByteStack;
    /// <summary>
    /// Определяет методы распаковки и упаковки для выбранного типа
    /// </summary>
    /// <typeparam name="T">target type</typeparam>
    public interface IPackagerMethod<T> : IPackagerMethodGeneric
    {
        /// <summary>
        /// Упаковывает данные в буфер
        /// </summary>
        /// <param name="writer">packaging interface</param>
        /// <param name="source">packing data</param>
        void PackUP(ref StackWriter writer, T source);
        /// <summary>
        /// Распаковка данных из буфера
        /// </summary>
        /// <param name="reader">unpacking interface</param>
        /// <param name="destination">data to be unpacked</param>
        void UnPack(ref StackReader reader, out T destination);
        /// <summary>
        /// Рассчитайте необходимый размер при упаковке
        /// </summary>
        /// <param name="meter">buffer length measurement interface</param>
        /// <param name="source">the object for which you need to calculate the size size</param>
        void GetSize(ref StackMeter meter, T source);
    }
}