using System;

namespace PeNet.Structures
{
    /// <summary>
    /// COM+ 2.0 (CLI) Header
    /// https://www.codeproject.com/Articles/12585/The-NET-File-Format
    /// </summary>
    public class ImageCor20Header : AbstractStructure
    {
        private ImageDataDirectory? _metaData;
        private ImageDataDirectory? _resources;
        private ImageDataDirectory? _strongSignatureNames;
        private ImageDataDirectory? _codeManagerTable;
        private ImageDataDirectory? _vTableFixups;
        private ImageDataDirectory? _exportAddressTableJumps;
        private ImageDataDirectory? _managedNativeHeader;

        /// <summary>
        /// Create a new instance of an COM+ 2 (CLI) header.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset to the COM+ 2 (CLI) header in the byte array.</param>
        public ImageCor20Header(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// Size of the structure.
        /// </summary>
        public uint Cb
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        /// Major runtime version of the CRL.
        /// </summary>
        public ushort MajorRuntimeVersion
        {
            get => PeFile.ReadUShort(Offset + 0x4);
            set => PeFile.WriteUShort(Offset + 0x4, value);
        }

        /// <summary>
        /// Minor runtime version of the CRL.
        /// </summary>
        public ushort MinorRuntimeVersion
        {
            get => PeFile.ReadUShort(Offset + 0x6);
            set => PeFile.WriteUShort(Offset + 0x6, value);
        }

        /// <summary>
        /// Meta data directory.
        /// </summary>
        public ImageDataDirectory? MetaData
        {
            get
            {
                if (_metaData != null)
                    return _metaData;

                _metaData = SetImageDataDirectory(PeFile, Offset + 0x8);
                return _metaData;
            }
        }
        
        /// <summary>
        /// COM image flags.
        /// </summary>
        public uint Flags
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }

        /// <summary>
        /// Represents the managed entry point if COMIMAGE_FLAGS_NATIVE_ENTRYPOINT is not set.
        /// Union with EntryPointRVA.
        /// </summary>
        public uint EntryPointToken
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        /// Represents an RVA to an native entry point if the COMIMAGE_FLAGS_NATIVE_ENTRYPOINT is set.
        /// Union with EntryPointToken.
        /// </summary>
        public uint EntryPointRVA
        {
            get => EntryPointToken;
            set => EntryPointToken = value;
        }

        /// <summary>
        /// Resource data directory.
        /// </summary>
        public ImageDataDirectory? Resources
        {
            get
            {
                _resources ??= SetImageDataDirectory(PeFile, Offset + 0x18);
                return _resources;
            }
        }

        /// <summary>
        /// Strong names signature directory.
        /// </summary>
        public ImageDataDirectory? StrongNameSignature
        {
            get
            {
                _strongSignatureNames ??= SetImageDataDirectory(PeFile, Offset + 0x20);
                return _strongSignatureNames;
            }
        }

        /// <summary>
        /// Code manager table directory.
        /// </summary>
        public ImageDataDirectory? CodeManagerTable
        {
            get
            {
                _codeManagerTable ??= SetImageDataDirectory(PeFile, Offset + 0x28);
                return _codeManagerTable;
            }
        }

        /// <summary>
        /// Virtual table fix up directory.
        /// </summary>
        public ImageDataDirectory? VTableFixups
        {
            get
            {
                _vTableFixups ??= SetImageDataDirectory(PeFile, Offset + 0x30);
                return _vTableFixups;
            }
        }

        /// <summary>
        /// Export address table jump directory.
        /// </summary>
        public ImageDataDirectory? ExportAddressTableJumps
        {
            get
            {
                _exportAddressTableJumps ??= SetImageDataDirectory(PeFile, Offset + 0x38);
                return _exportAddressTableJumps;
            }
        }

        /// <summary>
        /// Managed native header directory.
        /// </summary>
        public ImageDataDirectory? ManagedNativeHeader
        {
            get
            {
                _managedNativeHeader ??= SetImageDataDirectory(PeFile, Offset + 0x40);
                return _managedNativeHeader;
            }
        }

        private ImageDataDirectory? SetImageDataDirectory(IRawFile peFile, long offset)
        {
            try
            {
                return new ImageDataDirectory(peFile, offset);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}