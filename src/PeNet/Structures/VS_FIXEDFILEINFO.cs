using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Language and code page independent version information about the PE file.
    /// </summary>
    public class VS_FIXEDFILEINFO : AbstractStructure
    {
        public VS_FIXEDFILEINFO(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        /// <summary>
        /// Constant 0xFEEF04BD. Used to search for the VS_FIXEDFILEINFO structure.
        /// </summary>
        public uint dwSignature
        {
            get => Buff.BytesToUInt32(Offset);
            set => Buff.SetUInt32(Offset, value);
        }

        /// <summary>
        /// Version number of the VS_FIXEDFILEINFO structure. The higher word of the member
        /// represents the major version, the lower word the minor version.
        /// </summary>
        public uint dwStrucVersion
        {
            get => Buff.BytesToUInt32(Offset + 0x4);
            set => Buff.SetUInt32(Offset + 0x4, value);
        }

        /// <summary>
        /// Most significant 32 bits of the version of the PE file. 
        /// </summary>
        public uint dwFileVersionMS
        {
            get => Buff.BytesToUInt32(Offset + 0x8);
            set => Buff.SetUInt32(Offset + 0x8, value);
        }

        /// <summary>
        /// Least significant 32 bits of the version of the PE file. 
        /// </summary>
        public uint dwFileVersionLS
        {
            get => Buff.BytesToUInt32(Offset + 0xC);
            set => Buff.SetUInt32(Offset + 0xC, value);
        }

        /// <summary>
        /// Most significant 32 bits of the version of the product the PE file was distributed with. 
        /// </summary>
        public uint dwProductVersionMS
        {
            get => Buff.BytesToUInt32(Offset + 0x10);
            set => Buff.SetUInt32(Offset + 0x10, value);
        }

        /// <summary>
        /// Least significant 32 bits of the version of the product the PE file was distributed with. 
        /// </summary>
        public uint dwProductVersionLS
        {
            get => Buff.BytesToUInt32(Offset + 0x14);
            set => Buff.SetUInt32(Offset + 0x14, value);
        }

        /// <summary>
        /// Bitmask to specify the valid bits in dwFileFlags. Bits are only valid if present
        /// on file creation.
        /// </summary>
        public uint dwFileFlagsMask
        {
            get => Buff.BytesToUInt32(Offset + 0x18);
            set => Buff.SetUInt32(Offset + 0x18, value);
        }

        /// <summary>
        /// Bitmask to specify attributes of the file.
        /// </summary>
        public uint dwFileFlags
        {
            get => Buff.BytesToUInt32(Offset + 0x1C);
            set => Buff.SetUInt32(Offset + 0x1C, value);
        }

        /// <summary>
        /// Each bit represents a different OS for which the file
        /// is intended. Only one can be set.
        /// </summary>
        public uint dwFileOS
        {
            get => Buff.BytesToUInt32(Offset + 0x20);
            set => Buff.SetUInt32(Offset + 0x20, value);
        }

        /// <summary>
        /// General type of the file, like DLL, driver, ...
        /// </summary>
        public uint dwFileType
        {
            get => Buff.BytesToUInt32(Offset + 0x24);
            set => Buff.SetUInt32(Offset + 0x24, value);
        }

        /// <summary>
        /// More specific type than dwFileType like display driver, sound driver, ...
        /// </summary>
        public uint dwFileSubType
        {
            get => Buff.BytesToUInt32(Offset + 0x28);
            set => Buff.SetUInt32(Offset + 0x28, value);
        }

        /// <summary>
        /// Most significant 32 bits of the files creation date and time.
        /// </summary>
        public uint dwFileDateMS
        {
            get => Buff.BytesToUInt32(Offset + 0x2C);
            set => Buff.SetUInt32(Offset + 0x2C, value);
        }

        /// <summary>
        /// Least significant 32 bits of the files creation date and time.
        /// </summary>
        public uint dwFileDateLS
        {
            get => Buff.BytesToUInt32(Offset + 0x30);
            set => Buff.SetUInt32(Offset + 0x30, value);
        }

    }
}