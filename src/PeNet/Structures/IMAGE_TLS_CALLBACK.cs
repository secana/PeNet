using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Thread Local Storage callback.
    /// </summary>
    public class IMAGE_TLS_CALLBACK : AbstractStructure
    {
        private readonly bool _is64Bit;

        /// <summary>
        /// Create a new TLS callback structure.
        /// </summary>
        /// <param name="buff">PE file as byte buffer.</param>
        /// <param name="offset">Offset of the TLS callback structure in the buffer.</param>
        /// <param name="is64Bit">Flag is the PE file is 64 Bit.</param>
        public IMAGE_TLS_CALLBACK(byte[] buff, uint offset, bool is64Bit) 
            : base(buff, offset)
        {
            _is64Bit = is64Bit;
        }

        /// <summary>
        /// Address of actual callback code.
        /// </summary>
        public ulong Callback
        {
            get => _is64Bit ? PeFile.BytesToUInt64(Offset + 0) : PeFile.ReadUInt(Offset + 0);
            set
            {
                if(_is64Bit)
                    PeFile.SetUInt64(Offset + 0, value);
                else
                    PeFile.WriteUInt(Offset + 0, (uint) value);
            }
        }
    }
}