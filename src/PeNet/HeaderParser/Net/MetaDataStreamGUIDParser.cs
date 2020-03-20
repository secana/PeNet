using PeNet.FileParser;
using PeNet.Header.Net;

namespace PeNet.HeaderParser.Net
{
    internal class MetaDataStreamGuidParser : SafeParser<MetaDataStreamGuid>
    {
        private readonly uint _size;

        public MetaDataStreamGuidParser(
            IRawFile peFile,
            long offset,
            uint size
        )
            : base(peFile, offset)

        {
            _size = size;
        }

        protected override MetaDataStreamGuid ParseTarget()
        {
            return new MetaDataStreamGuid(PeFile, Offset, _size);
        }
    }
}