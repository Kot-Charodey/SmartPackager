using System;

namespace SmartPackager.Automatic
{
    /// <summary>
    /// Указывает автоматическому генератору упаковки, что стоит упаковать частные поля
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class SearchPrivateFieldsAttribute : Attribute
    {
    }
}
