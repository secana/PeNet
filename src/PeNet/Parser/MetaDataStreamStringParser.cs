using PeNet.FileParser;
using PeNet.Structures;

namespace PeNet.Parser
{
    internal class MetaDataStreamStringParser : SafeParser<IMETADATASTREAM_STRING>
    {
        private readonly uint _size;

        public MetaDataStreamStringParser(
            IRawFile peFile, 
            long offset,
            uint size
            ) 
            : base(peFile, offset)
        {
            _size = size;
        }

        protected override IMETADATASTREAM_STRING ParseTarget()
        {
            return new METADATASTREAM_STRING(PeFile, Offset, _size);
        }
    }
}