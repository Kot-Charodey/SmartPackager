using System;

namespace SmartPackager
{
    /// <summary>
    /// Auxiliary wrapper type for "IPackagerMethod"
    /// </summary>
    public interface IPackagerMethodGeneric
    {
        /// <summary>
        /// Target Type
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// Does the type have a fixed size
        /// </summary>
        bool IsFixedSize { get; }
    }
}