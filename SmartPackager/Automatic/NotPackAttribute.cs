using System;

namespace SmartPackager.Automatic
{
    /// <summary>
    /// Specifies that this field or property should not be packaged
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NotPackAttribute : Attribute
    {
    }
}
