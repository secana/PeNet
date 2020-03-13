using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The ImageDataDirectory struct represents the data directory,
    /// </summary>
    public class ImageDataDirectory : AbstractStructure
    {
        /// <summary>
        ///     Create a new ImageDataDirectory object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset to the data directory in the binary.</param>
        public ImageDataDirectory(IRawFile peFile, long offset)
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

    /// <summary>
    ///     The data directory indices used to resolve
    ///     which directory is which.
    /// </summary>
    public enum DataDirectoryType
    {
        /// <summary>
        ///     Export directory.
        /// </summary>
        Export = 0,

        /// <summary>
        ///     Import directory.
        /// </summary>
        Import = 1,

        /// <summary>
        ///     Resource directory.
        /// </summary>
        Resource = 2,

        /// <summary>
        ///     Exception directory for x64.
        /// </summary>
        Exception = 3,

        /// <summary>
        ///     Security directory.
        /// </summary>
        Security = 4,

        /// <summary>
        ///     Relocation directory.
        /// </summary>
        BaseReloc = 5,

        /// <summary>
        ///     Debug directory.
        /// </summary>
        Debug = 6,

        /// <summary>
        ///     Copyright directory (useless).
        /// </summary>
        Copyright = 7,

        /// <summary>
        ///     Global Pointer directory. Only interesting for Itanium systems.
        /// </summary>
        Globalptr = 8,

        /// <summary>
        ///     Thread Local Storage directory.
        /// </summary>
        TLS = 9,

        /// <summary>
        ///     Load Config directory.
        /// </summary>
        LoadConfig = 0xA,

        /// <summary>
        ///     Bound Import directory. Precomputed import addresses
        ///     to speed up module loading.
        /// </summary>
        BoundImport = 0xB,

        /// <summary>
        ///     Import Address Table directory.
        /// </summary>
        IAT = 0xC,

        /// <summary>
        ///     Delayed Import directory. Imports which are loaded
        ///     with a delay for performance reasons.
        /// </summary>
        DelayImport = 0xD,

        /// <summary>
        ///     COM Descriptor directory. For the .Net Header
        /// </summary>
        ComDescriptor = 0xE,

        /// <summary>
        ///     Reserved for future use.
        /// </summary>
        Reserved = 0xF
    }
}