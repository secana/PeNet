using System;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     The resource directory contains icons, mouse pointer, string
    ///     language files etc. which are used by the application.
    /// </summary>
    public class IMAGE_RESOURCE_DIRECTORY : AbstractStructure
    {
        /// <summary>
        ///     Array with the different directory entries.
        /// </summary>
        public readonly IMAGE_RESOURCE_DIRECTORY_ENTRY[] DirectoryEntries;

        /// <summary>
        ///     Create a new IMAGE_RESOURCE_DIRECTORY object.
        /// </summary>
        /// <param name="buff">A PE file as a byte array.</param>
        /// <param name="offset">Raw offset to the resource directory.</param>
        /// <param name="resourceDirOffset">Raw offset to the resource directory entries.</param>
        public IMAGE_RESOURCE_DIRECTORY(byte[] buff, uint offset, uint resourceDirOffset)
            : base(buff, offset)
        {
            DirectoryEntries = ParseDirectoryEntries(resourceDirOffset);
        }

        /// <summary>
        ///     Characteristics.
        /// </summary>
        public uint Characteristics
        {
            get => Buff.BytesToUInt32(Offset);
            set => Buff.SetUInt32(Offset, value);
        }

        /// <summary>
        ///     Time and date stamp.
        /// </summary>
        public uint TimeDateStamp
        {
            get => Buff.BytesToUInt32(Offset + 0x4);
            set => Buff.SetUInt32(Offset + 0x4, value);
        }

        /// <summary>
        ///     Major version.
        /// </summary>
        public ushort MajorVersion
        {
            get => Buff.BytesToUInt16(Offset + 0x8);
            set => Buff.SetUInt16(Offset + 0x8, value);
        }

        /// <summary>
        ///     Minor version.
        /// </summary>
        public ushort MinorVersion
        {
            get => Buff.BytesToUInt16(Offset + 0xa);
            set => Buff.SetUInt16(Offset + 0xa, value);
        }

        /// <summary>
        ///     Number of named entries.
        /// </summary>
        public ushort NumberOfNameEntries
        {
            get => Buff.BytesToUInt16(Offset + 0xc);
            set => Buff.SetUInt16(Offset + 0xc, value);
        }

        /// <summary>
        ///     Number of ID entries.
        /// </summary>
        public ushort NumberOfIdEntries
        {
            get => Buff.BytesToUInt16(Offset + 0xe);
            set => Buff.SetUInt16(Offset + 0xe, value);
        }

        private IMAGE_RESOURCE_DIRECTORY_ENTRY[] ParseDirectoryEntries(uint resourceDirOffset)
        {
            if (SanityCheckFailed())
                return null;

            var entries = new IMAGE_RESOURCE_DIRECTORY_ENTRY[NumberOfIdEntries + NumberOfNameEntries];

            for (var index = 0; index < entries.Length; index++)
            {
                try
                {
                    entries[index] = new IMAGE_RESOURCE_DIRECTORY_ENTRY(Buff, (uint) index*8 + Offset + 16,
                        resourceDirOffset);
                }
                catch (IndexOutOfRangeException)
                {
                    entries[index] = null;
                }
            }

            return entries;
        }

        private bool SanityCheckFailed()
        {
            // There exists the case that only some second/third stage directories are valid and others
            // are not. For this case try to parse at least the valid ones. In that case
            // accessing properties throws an "IndexOutOfRange" exception.
            // Example (malicious!): 9d5eb5ac899764d5ed30cc93df8d645e598e2cbce53ae7bb081ded2c38286d1e
            try
            {
                if (NumberOfIdEntries + NumberOfNameEntries >= 1000)
                    return true;
            }
            catch (Exception)
            {
                return true;
            }

            return false;
        }
    }
}