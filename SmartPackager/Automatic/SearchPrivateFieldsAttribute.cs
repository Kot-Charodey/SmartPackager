using System;

namespace SmartPackager.Automatic
{
    /// <summary>
    /// Indicates to the automatic packaging generator that it is worth packing private fields
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class SearchPrivateFieldsAttribute : Attribute
    {
    }
}
