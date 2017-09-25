using PeNet.Structures;

namespace PeNet.Parser
{
    internal class MetaDataStreamUSParser : SafeParser<IMETADATASTREAM_US>
    {
        private readonly uint _size;

        public MetaDataStreamUSParser(byte[] buff, uint offset, uint size) 
            : base(buff, offset)
        {
            _size = size;
        }

        protected override IMETADATASTREAM_US ParseTarget()
        {
            return new METADATASTREAM_US(_buff, _offset, _size);
        }
    }
}