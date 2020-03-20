using PeNet.FileParser;
using PeNet.Header.Resource;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The ImageDebugDirectory hold debug information
    ///     about the PE file.
    /// </summary>
    public class ImageDebugDirectory : AbstractStructure
    {
        private CvInfoPdb70? _cvInfoPdb70;

        /// <summary>
        ///     Create a new ImageDebugDirectory object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset to the debug structure in the PE file.</param>
        public ImageDebugDirectory(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Characteristics of the debug information.
        /// </summary>
        public uint Characteristics
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     Time and date stamp
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
            get => PeFile.ReadUShort(Offset + 0xa);
            set => PeFile.WriteUShort(Offset + 0xa, value);
        }

        /// <summary>
        ///     Type
        ///     1: Coff
        ///     2: CV-PDB
        ///     9: Borland
        /// </summary>
        public uint Type
        {
            get => PeFile.ReadUInt(Offset + 0xc);
            set => PeFile.WriteUInt(Offset + 0xc, value);
        }

        /// <summary>
        ///     Size of data.
        /// </summary>
        public uint SizeOfData
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }

        /// <summary>
        ///     Address of raw data.
        /// </summary>
        public uint AddressOfRawData
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        ///     Pointer to raw data.
        /// </summary>
        public uint PointerToRawData
        {
            get => PeFile.ReadUInt(Offset + 0x18);
            set => PeFile.WriteUInt(Offset + 0x18, value);
        }

        /// <summary>
        /// PDB information if the "Type" is IMAGE_DEBUG_TYPE_CODEVIEW.
        /// </summary>
        public CvInfoPdb70? CvInfoPdb70
        {
            get
            {
                if (Type != 2) // Type IMAGE_DEBUG_TYPE_CODEVIEW
                    return null;

                _cvInfoPdb70 ??= new CvInfoPdb70(
                    PeFile, 
                    PointerToRawData);

                return _cvInfoPdb70;
            }
        }
    }
}