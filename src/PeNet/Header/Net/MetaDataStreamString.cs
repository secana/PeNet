using PeNet.FileParser;

namespace PeNet.Header.Net
{
    /// <summary>
    /// Represents the "String" meta data stream from the .Net header which 
    /// contains all application interal strings.
    /// </summary>
    public class MetaDataStreamString : AbstractStructure
    {
        private readonly uint _size;
        
        public MetaDataStreamString(IRawFile peFile, long offset, uint size) 
            : base(peFile, offset)
        {
            _size = size;
        }

        /// <summary>
        /// Return the string at the index from the stream.
        /// </summary>
        /// <param name="index">Index of the string to return.</param>
        /// <returns>String at the position index.</returns>
        public string GetStringAtIndex(uint index)
        {
            return PeFile.ReadAsciiString(Offset + index);
        }
    }
}