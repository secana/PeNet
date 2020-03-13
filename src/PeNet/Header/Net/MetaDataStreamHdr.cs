using PeNet.FileParser;

namespace PeNet.Header.Net
{
    /// <summary>
    /// The Meta Data Stream Header contains information about data streams (sections)
    /// in a .Net assembly.
    /// </summary>
    public class MetaDataStreamHdr : AbstractStructure
    {
        internal uint HeaderLength => GetHeaderLength();

        /// <summary>
        /// Create a new Meta Data Stream Header instance from a byte array.
        /// </summary>
        /// <param name="peFile">PE file which contains a Meta Data Stream Header.</param>
        /// <param name="offset">Offset in the buffer, where the header starts.</param>
        public MetaDataStreamHdr(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// Relative offset (from Meta Data Header) to 
        /// the stream.
        /// </summary>
        public uint RelOffset
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        /// Size of the stream content.
        /// </summary>
        public uint Size
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        /// Name of the stream.
        /// </summary>
        public string StreamName => PeFile.ReadAsciiString(Offset + 0x8);

        private uint GetHeaderLength()
        {
            var maxHeaderLength = 100;
            var headerLength = 0;
            for (var inHdrOffset = 8; inHdrOffset < maxHeaderLength; inHdrOffset++)
            {
                if (PeFile.ReadByte(Offset + inHdrOffset) == 0x00)
                {
                    headerLength = inHdrOffset;
                    break;
                }
                    
            }

            return (uint) AddHeaderPaddingLength(headerLength);
        }

        private int AddHeaderPaddingLength(int headerLength)
        {
            if (headerLength%4 == 0)
                return headerLength + 4;
            else
            {
                return headerLength + (4-(headerLength%4));
            }
        }
    }
}