using System;

namespace SmartPackager
{
    /// <summary>
    /// Defines the unpacking and packing methods for the selected type
    /// </summary>
    /// <typeparam name="T">target type</typeparam>
    public unsafe interface IPackagerMethod<T> : IPackagerMethodGeneric
    {
        /// <summary>
        /// Packs an object into an array of bytes
        /// </summary>
        /// <param name="destination">pointer to the beginning of the recording</param>
        /// <param name="source">object to record</param>
        /// <returns>how many bytes were written</returns>
        int PackUP(byte* destination, T source);
        /// <summary>
        /// Unpacks an object from an array of bytes
        /// </summary>
        /// <param name="source">pointer to the beginning of reading</param>
        /// <param name="destination">finished object</param>
        /// <returns>how many bytes were read</returns>
        int UnPack(byte* source, out T destination);
        /// <summary>
        /// </summary>
        /// <param name="source">target</param>
        /// <returns>Object size in bytes</returns>
        int GetSize(T source);
    }
}