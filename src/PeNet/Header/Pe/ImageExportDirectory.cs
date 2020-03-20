using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The export directory contains all exported function, symbols etc.
    ///     which can be used by other module.
    /// </summary>
    public class ImageExportDirectory : AbstractStructure
    {
        /// <summary>
        ///     Create a new ImageExportDirectory object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset of the export directory in the PE file.</param>
        public ImageExportDirectory(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     The characteristics of the export directory.
        /// </summary>
        public uint Characteristics
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     Time and date stamp.
        /// </summary>
        public uint TimeDateStamp
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Major Version.
        /// </summary>
        public ushort MajorVersion
        {
            get => PeFile.ReadUShort(Offset + 0x8);
            set => PeFile.WriteUShort(Offset + 0x8, value);
        }

        /// <summary>
        ///     Minor Version.
        /// </summary>
        public ushort MinorVersion
        {
            get => PeFile.ReadUShort(Offset + 0xA);
            set => PeFile.WriteUShort(Offset + 0xA, value);
        }

        /// <summary>
        ///     Name.
        /// </summary>
        public uint Name
        {
            get => PeFile.ReadUInt(Offset + 0xC);
            set => PeFile.WriteUInt(Offset + 0xC, value);
        }

        /// <summary>
        ///     Base.
        /// </summary>
        public uint Base
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }

        /// <summary>
        ///     Number of exported functions.
        /// </summary>
        public uint NumberOfFunctions
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        ///     Number of exported names.
        /// </summary>
        public uint NumberOfNames
        {
            get => PeFile.ReadUInt(Offset + 0x18);
            set => PeFile.WriteUInt(Offset + 0x18, value);
        }

        /// <summary>
        ///     RVA to the addresses of the functions.
        /// </summary>
        public uint AddressOfFunctions
        {
            get => PeFile.ReadUInt(Offset + 0x1C);
            set => PeFile.WriteUInt(Offset + 0x1C, value);
        }

        /// <summary>
        ///     RVA to the addresses of the names.
        /// </summary>
        public uint AddressOfNames
        {
            get => PeFile.ReadUInt(Offset + 0x20);
            set => PeFile.WriteUInt(Offset + 0x20, value);
        }

        /// <summary>
        ///     RVA to the name ordinals.
        /// </summary>
        public uint AddressOfNameOrdinals
        {
            get => PeFile.ReadUInt(Offset + 0x24);
            set => PeFile.WriteUInt(Offset + 0x24, value);
        }
    }
}