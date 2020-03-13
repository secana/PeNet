using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The ImageResourceDataEntry points to the data of
    ///     the resources in the PE file like version info, strings etc.
    /// </summary>
    public class ImageResourceDataEntry : AbstractStructure
    {
        /// <summary>
        ///     Construct a ImageResourceDataEntry at a given offset.
        /// </summary>
        /// <param name="peFile">PE file.</param>
        /// <param name="offset">Offset to the structure in the file.</param>
        public ImageResourceDataEntry(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Offset to the data of the resource.
        /// </summary>
        public uint OffsetToData
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     Size of the resource data.
        /// </summary>
        public uint Size1
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Code Page
        /// </summary>
        public uint CodePage
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }

        /// <summary>
        ///     Reserved
        /// </summary>
        public uint Reserved
        {
            get => PeFile.ReadUInt(Offset + 0xC);
            set => PeFile.WriteUInt(Offset + 0xC, value);
        }
    }
}