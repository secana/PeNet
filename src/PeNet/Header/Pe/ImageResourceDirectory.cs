using System;
using System.Collections.Generic;
using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The resource directory contains icons, mouse pointer, string
    ///     language files etc. which are used by the application.
    /// </summary>
    public class ImageResourceDirectory : AbstractStructure
    {
        private bool _entriesParsed;
        private readonly long _resourceDirOffset;
        private List<ImageResourceDirectoryEntry?>? _directoryEntries;
        private readonly long _resourceDirLength;

        /// <summary>
        ///     Array with the different directory entries.
        /// </summary>
        public List<ImageResourceDirectoryEntry?>? DirectoryEntries
        {
            get
            {
                if (_entriesParsed)
                    return _directoryEntries;

                _entriesParsed = true;
                _directoryEntries = ParseDirectoryEntries(_resourceDirOffset);
                return _directoryEntries;
            }
        }

        /// <summary>
        ///     Create a new ImageResourceDirectory object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="parent">Higher level directory entry which wraps this directory.</param>
        /// <param name="offset">Raw offset to the resource directory.</param>
        /// <param name="resourceDirOffset">Raw offset to the resource directory entries.</param>
        /// <param name="resourceDirLength">Length of the resource directory entries.</param>
        public ImageResourceDirectory(IRawFile peFile, ImageResourceDirectoryEntry? parent, long offset, long resourceDirOffset, long resourceDirLength)
            : base(peFile, offset)
        {
            Parent = parent;
            _resourceDirOffset = resourceDirOffset;
            _resourceDirLength = resourceDirLength;
        }

        /// <summary>
        ///     Backwards connection to the parent.
        ///     Higher level directory entry which wraps this directory.
        /// </summary>
        public ImageResourceDirectoryEntry? Parent { get; }

        /// <summary>
        ///     Characteristics.
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
        ///     Major version.
        /// </summary>
        public ushort MajorVersion
        {
            get => PeFile.ReadUShort(Offset + 0x8);
            set => PeFile.WriteUShort(Offset + 0x8, value);
        }

        /// <summary>
        ///     Minor version.
        /// </summary>
        public ushort MinorVersion
        {
            get => PeFile.ReadUShort(Offset + 0xa);
            set => PeFile.WriteUShort(Offset + 0xa, value);
        }

        /// <summary>
        ///     Number of named entries.
        /// </summary>
        public ushort NumberOfNameEntries
        {
            get => PeFile.ReadUShort(Offset + 0xc);
            set => PeFile.WriteUShort(Offset + 0xc, value);
        }

        /// <summary>
        ///     Number of ID entries.
        /// </summary>
        public ushort NumberOfIdEntries
        {
            get => PeFile.ReadUShort(Offset + 0xe);
            set => PeFile.WriteUShort(Offset + 0xe, value);
        }

        private List<ImageResourceDirectoryEntry?> ParseDirectoryEntries(long resourceDirOffset)
        {
            var numEntries = NumberOfIdEntries + NumberOfNameEntries;

            var entries = new List<ImageResourceDirectoryEntry?>(numEntries);

            for (var index = 0; index < numEntries; index++)
            {
                try
                {
                    var entry = new ImageResourceDirectoryEntry(PeFile, this, (uint) index * 8 + Offset + 16,
                        resourceDirOffset);

                    if(SanityCheckFailed(entry))
                        break;
                    entries.Add(entry);
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }
            }

            return entries;
        }

        bool SanityCheckFailed(ImageResourceDirectoryEntry? rd)
        {
            if (rd == null)
                return true;

            if(rd.IsNamedEntry && rd.NameResolved == null )
                return true;

            if (rd.IsNamedEntry && rd.NameResolved == "unknown")
                return true;

            if(rd.OffsetToDirectory > _resourceDirLength)
                return true;

            return false;
        }
    }

}
