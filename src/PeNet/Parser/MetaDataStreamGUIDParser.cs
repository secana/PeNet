using PeNet.Structures;

namespace PeNet.Parser
{
    internal class MetaDataStreamGUIDParser : SafeParser<IMETADATASTREAM_GUID>
    {
        private readonly uint _size;

        public MetaDataStreamGUIDParser(
            IRawFile peFile,
            long offset,
            uint size
        )
            : base(peFile, offset)

        {
            _size = size;
        }

        protected override IMETADATASTREAM_GUID ParseTarget()
        {
            return new METADATASTREAM_GUID(PeFile, Offset, _size);
        }
    }
}