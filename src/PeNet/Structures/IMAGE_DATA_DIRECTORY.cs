using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     The IMAGE_DATA_DIRECTORY struct represents the data directory,
    /// </summary>
    public class IMAGE_DATA_DIRECTORY : AbstractStructure
    {
        /// <summary>
        ///     Create a new IMAGE_DATA_DIRECTORY object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset to the data directory in the binary.</param>
        public IMAGE_DATA_DIRECTORY(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     RVA of the table.
        /// </summary>
        public uint VirtualAddress
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     Table size in bytes.
        /// </summary>
        public uint Size
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }
    }
}