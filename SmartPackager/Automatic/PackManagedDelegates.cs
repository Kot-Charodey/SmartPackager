namespace SmartPackager.Automatic
{
    using SmartPackager.ByteStack;

    /// <see cref="PackManagedAutomatic{T}"/>
    internal delegate void GenericGetSize<T>(ref StackMeter meter, T source);

    /// <see cref="PackManagedAutomatic{T}"/>
    internal delegate void GenericPackUP<T>(ref StackWriter writer, T source);

    /// <see cref="PackManagedAutomatic{T}"/>
    internal delegate void GenericUnPack<T>(ref StackReader reader, ref T destination);




    /// <see cref="PackManagedAutomatic{T}"/>
    internal delegate void GenericGetSizeMain<T>(ref StackMeter meter, T source, GenericGetSize<T> getSize);
    /// <see cref="PackManagedAutomatic{T}"/>
    internal delegate void GenericPackUPMain<T>(ref StackWriter writer, T source, GenericPackUP<T> packUp);
    /// <see cref="PackManagedAutomatic{T}"/>
    internal delegate void GenericUnPackMain<T>(ref StackReader reader, out T destination, GenericUnPack<T> unPack);
}
