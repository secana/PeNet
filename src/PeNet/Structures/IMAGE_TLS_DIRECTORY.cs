using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Thread Local Storage Directory.
    /// </summary>
    public class IMAGE_TLS_DIRECTORY : AbstractStructure
    {
        private readonly bool _is64Bit;

        /// <summary>
        /// Create new TLS directory structure.
        /// </summary>
        /// <param name="buff">PE file as binary buffer.</param>
        /// <param name="offset">Offset to TLS structure in buffer.</param>
        /// <param name="is64Bit">Flag if the PE file is 64 Bit.</param>
        public IMAGE_TLS_DIRECTORY(byte[] buff, uint offset, bool is64Bit) 
            : base(buff, offset)
        {
            _is64Bit = is64Bit;
        }

        /// <summary>
        /// Start address of the raw data.
        /// </summary>
        public ulong StartAddressOfRawData
        {
            get
            {
                return _is64Bit ? Buff.BytesToUInt64(Offset + 0) : Buff.BytesToUInt32(Offset + 0);
            }
            set
            {
                if (_is64Bit)
                    Buff.SetUInt64(Offset + 0, value);
                else
                    Buff.SetUInt32(Offset + 0, (uint) value);
            } 
        }

        /// <summary>
        /// End address of the raw data.
        /// </summary>
        public ulong EndAddressOfRawData
        {
            get
            {
                return _is64Bit ? Buff.BytesToUInt64(Offset + 8) : Buff.BytesToUInt32(Offset + 4);
            }
            set
            {
                if(_is64Bit)
                    Buff.SetUInt64(Offset + 8, value);
                else
                    Buff.SetUInt32(Offset + 4, (uint) value);
            }
        }

        /// <summary>
        /// Address of index (pointer to TLS index).
        /// </summary>
        public ulong AddressOfIndex
        {
            get
            {
                return _is64Bit ? Buff.BytesToUInt64(Offset + 0x10) : Buff.BytesToUInt32(Offset + 8);
            }
            set
            {
                if(_is64Bit)
                    Buff.SetUInt64(Offset + 0x10, value);
                else
                    Buff.SetUInt32(Offset + 8, (uint) value);
            }
        }

        /// <summary>
        /// Address of the callbacks.
        /// </summary>
        public ulong AddressOfCallBacks
        {
            get
            {
                return _is64Bit ? Buff.BytesToUInt64(Offset + 0x18) : Buff.BytesToUInt32(Offset + 0x0c);
            }
            set
            {
                if(_is64Bit)
                    Buff.SetUInt64(Offset + 0x18, value);
                else
                    Buff.SetUInt32(Offset + 0x0c, (uint) value);
            }
        }

        /// <summary>
        /// Size of zero fill.
        /// </summary>
        public uint SizeOfZeroFill
        {
            get
            {
                return _is64Bit ? Buff.BytesToUInt32(Offset + 0x20) : Buff.BytesToUInt32(Offset + 0x10);
            }
            set
            {
                if(_is64Bit)
                    Buff.SetUInt32(Offset + 0x20, value);
                else
                    Buff.SetUInt32(Offset + 0x10, value);
            }
        }

        /// <summary>
        /// Characteristics.
        /// </summary>
        public uint Characteristics
        {
            get
            {
                return _is64Bit ? Buff.BytesToUInt32(Offset + 0x24) : Buff.BytesToUInt32(Offset + 0x14);
            }
            set
            {
                if(_is64Bit)
                    Buff.SetUInt32(Offset + 0x24, value);
                else
                    Buff.SetUInt32(Offset+0x14, value);
            }
        }

        /// <summary>
        /// List with parsed TLS callback structures.
        /// </summary>
        public IMAGE_TLS_CALLBACK[] TlsCallbacks { get; set; }
    }
}