using System;

namespace SmartPackager
{
    /// <summary>
    /// Вспомогательный тип оболочки для "IPackagerMethod"
    /// </summary>
    public interface IPackagerMethodGeneric
    {
        /// <summary>
        /// Целевой тип
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// Имеет ли тип фиксированный размер
        /// </summary>
        bool IsFixedSize { get; }
    }
}