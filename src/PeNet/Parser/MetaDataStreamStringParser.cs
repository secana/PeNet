using PeNet.Structures;

namespace PeNet.Parser
{
    internal class MetaDataStreamStringParser : SafeParser<IMETADATASTREAM_STRING>
    {
        private readonly uint _size;

        public MetaDataStreamStringParser(
            byte[] buff, 
            uint offset,
            uint size
            ) 
            : base(buff, offset)
        {
            _size = size;
        }

        protected override IMETADATASTREAM_STRING ParseTarget()
        {
            return new METADATASTREAM_STRING(_buff, _offset, _size);
        }
    }
}