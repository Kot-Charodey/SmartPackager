using System;

namespace SmartPackager.Automatic
{
    /// <summary>
    /// Указывает, что это поле или свойство не должно быть упаковано
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NotPackAttribute : Attribute
    {
    }
}
