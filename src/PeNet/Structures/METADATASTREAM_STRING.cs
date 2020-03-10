using PeNet.FileParser;

namespace PeNet.Structures
{
    public interface IMETADATASTREAM_STRING
    {
        /// <summary>
        /// Return the string at the index from the stream.
        /// </summary>
        /// <param name="index">Index of the string to return.</param>
        /// <returns>String at the position index.</returns>
        string GetStringAtIndex(uint index);

        /// <summary>
        ///     Creates a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>Optional header properties as a string.</returns>
        string ToString();
    }

    /// <inheritdoc cref="IMETADATASTREAM_STRING" />
    /// <summary>
    /// Represents the "String" meta data stream from the .Net header which 
    /// contains all application interal strings.
    /// </summary>
    public class METADATASTREAM_STRING : AbstractStructure, IMETADATASTREAM_STRING
    {
        private readonly uint _size;
        
        public METADATASTREAM_STRING(IRawFile peFile, long offset, uint size) 
            : base(peFile, offset)
        {
            _size = size;
        }

        public string GetStringAtIndex(uint index)
        {
            return PeFile.ReadAsciiString(Offset + index);
        }
    }
}