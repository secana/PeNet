using PeNet.Structures;

namespace PeNet.Parser
{
    internal class MetaDataStreamGUIDParser : SafeParser<IMETADATASTREAM_GUID>
    {
        private readonly uint _size;

        public MetaDataStreamGUIDParser(
            byte[] buff,
            uint offset,
            uint size
        )
            : base(buff, offset)

        {
            _size = size;
        }

        protected override IMETADATASTREAM_GUID ParseTarget()
        {
            return new METADATASTREAM_GUID(_buff, _offset, _size);
        }
    }
}