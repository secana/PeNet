using System;
using PeNet.FileParser;

namespace PeNet.Header.Resource
{
    /// <summary>
    ///     Information about Icons.
    /// </summary>
    public class Icon : AbstractStructure
    {
        public uint Size { get; }
        public uint Id { get; }

        /// <summary>
        ///     Creates a new Icon instance and sets Size and ID.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of the Icon image in the PE file.</param>
        /// <param name="size">Size of the Icon image in the PE file.</param>
        /// <param name="id">ID of the Icon.</param>
        public Icon(IRawFile peFile, long offset, uint size, uint id)
            : base(peFile, offset)
        {
            Size = size;
            Id = id;
        }

        /// <summary>
        ///     Byte span of the icon image.
        /// </summary>
        public Span<byte> AsSpan()
        {
            return PeFile.AsSpan(Offset, Size);
        }
    }
}
