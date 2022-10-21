using PeNet.Header.Pe;

namespace PeNet.Header.Resource
{
    /// <summary>
    ///     Record to handel location information.
    /// </summary>
    /// <param name="Resource">An ImageResourceDataEntry.</param>
    /// <param name="Offset">Offset of the ImageResourceDataEntry in the PE file.</param>
    /// <param name="Size">Size of the ImageResourceDataEntry in the PE file.</param>
    public record ResourceLocation(ImageResourceDataEntry Resource, uint Offset, uint Size)
    {
        public ImageResourceDataEntry Resource { get; } = Resource;
        public uint Offset { get; } = Offset;
        public uint Size { get; } = Size;
    }
}
