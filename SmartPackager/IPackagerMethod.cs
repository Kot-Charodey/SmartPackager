using System;

namespace SmartPackager
{
    public unsafe interface IPackagerMethod
    {
        /// <summary>
        /// Target Type
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// packs an object into an array of bytes
        /// </summary>
        /// <param name="destination">pointer to the beginning of the recording</param>
        /// <param name="source">object to record</param>
        /// <returns>how many bytes were written</returns>
        long PackUP(byte* destination, object source);
        /// <summary>
        /// unpacks an object from an array of bytes
        /// </summary>
        /// <param name="source">pointer to the beginning of reading</param>
        /// <param name="destination">finished object</param>
        /// <returns>how many bytes were read</returns>
        long UnPack(byte* source, out object destination);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">target</param>
        /// <returns>object size in bytes</returns>
        long GetSize(object source);
    }
}
