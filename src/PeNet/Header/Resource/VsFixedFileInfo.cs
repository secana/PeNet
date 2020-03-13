using PeNet.FileParser;

namespace PeNet.Header.Resource
{
    /// <summary>
    /// Language and code page independent version information about the PE file.
    /// </summary>
    public class VsFixedFileInfo : AbstractStructure
    {
        /// <summary>
        /// Create a new VsFixedFileInfo instance.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of the VsFixedFileInfo structure in the PE file.</param>
        public VsFixedFileInfo(IRawFile peFile, int offset) 
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// Constant 0xFEEF04BD. Used to search for the VsFixedFileInfo structure.
        /// </summary>
        public uint DwSignature
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        /// Version number of the VsFixedFileInfo structure. The higher word of the member
        /// represents the major version, the lower word the minor version.
        /// </summary>
        public uint DwStrucVersion
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        /// Most significant 32 bits of the version of the PE file. 
        /// </summary>
        public uint DwFileVersionMS
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }

        /// <summary>
        /// Least significant 32 bits of the version of the PE file. 
        /// </summary>
        public uint DwFileVersionLS
        {
            get => PeFile.ReadUInt(Offset + 0xC);
            set => PeFile.WriteUInt(Offset + 0xC, value);
        }

        /// <summary>
        /// Most significant 32 bits of the version of the product the PE file was distributed with. 
        /// </summary>
        public uint DwProductVersionMS
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }

        /// <summary>
        /// Least significant 32 bits of the version of the product the PE file was distributed with. 
        /// </summary>
        public uint DwProductVersionLS
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        /// Bitmask to specify the valid bits in dwFileFlags. Bits are only valid if present
        /// on file creation.
        /// </summary>
        public uint DwFileFlagsMask
        {
            get => PeFile.ReadUInt(Offset + 0x18);
            set => PeFile.WriteUInt(Offset + 0x18, value);
        }

        /// <summary>
        /// Bitmask to specify attributes of the file.
        /// </summary>
        public uint DwFileFlags
        {
            get => PeFile.ReadUInt(Offset + 0x1C);
            set => PeFile.WriteUInt(Offset + 0x1C, value);
        }

        /// <summary>
        /// Each bit represents a different OS for which the file
        /// is intended. Only one can be set.
        /// </summary>
        public uint DwFileOS
        {
            get => PeFile.ReadUInt(Offset + 0x20);
            set => PeFile.WriteUInt(Offset + 0x20, value);
        }

        /// <summary>
        /// General type of the file, like DLL, driver, ...
        /// </summary>
        public uint DwFileType
        {
            get => PeFile.ReadUInt(Offset + 0x24);
            set => PeFile.WriteUInt(Offset + 0x24, value);
        }

        /// <summary>
        /// More specific type than dwFileType like display driver, sound driver, ...
        /// </summary>
        public uint DwFileSubType
        {
            get => PeFile.ReadUInt(Offset + 0x28);
            set => PeFile.WriteUInt(Offset + 0x28, value);
        }

        /// <summary>
        /// Most significant 32 bits of the files creation date and time.
        /// </summary>
        public uint DwFileDateMS
        {
            get => PeFile.ReadUInt(Offset + 0x2C);
            set => PeFile.WriteUInt(Offset + 0x2C, value);
        }

        /// <summary>
        /// Least significant 32 bits of the files creation date and time.
        /// </summary>
        public uint DwFileDateLS
        {
            get => PeFile.ReadUInt(Offset + 0x30);
            set => PeFile.WriteUInt(Offset + 0x30, value);
        }

    }
}