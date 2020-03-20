using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    /// Thread Local Storage callback.
    /// </summary>
    public class ImageTlsCallback : AbstractStructure
    {
        private readonly bool _is64Bit;

        /// <summary>
        /// Create a new TLS callback structure.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of the TLS callback structure in the buffer.</param>
        /// <param name="is64Bit">Flag is the PE file is 64 Bit.</param>
        public ImageTlsCallback(IRawFile peFile, long offset, bool is64Bit) 
            : base(peFile, offset)
        {
            _is64Bit = is64Bit;
        }

        /// <summary>
        /// Address of actual callback code.
        /// </summary>
        public ulong Callback
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0) : PeFile.ReadUInt(Offset + 0);
            set
            {
                if(_is64Bit)
                    PeFile.WriteULong(Offset + 0, value);
                else
                    PeFile.WriteUInt(Offset + 0, (uint) value);
            }
        }
    }
}