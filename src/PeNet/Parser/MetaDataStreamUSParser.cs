using PeNet.Structures;

namespace PeNet.Parser
{
    internal class MetaDataStreamUSParser : SafeParser<IMETADATASTREAM_US>
    {
        private readonly uint _size;

        public MetaDataStreamUSParser(IRawFile peFile, long offset, uint size) 
            : base(peFile, offset)
        {
            _size = size;
        }

        protected override IMETADATASTREAM_US ParseTarget()
        {
            return new METADATASTREAM_US(PeFile, Offset, _size);
        }
    }
}