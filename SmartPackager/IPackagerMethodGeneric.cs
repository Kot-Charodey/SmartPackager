﻿using System;

namespace SmartPackager
{
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