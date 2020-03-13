using PeNet.FileParser;
using PeNet.Header.Net;

namespace PeNet.HeaderParser.Net
{
    internal class MetaDataStreamStringParser : SafeParser<MetaDataStreamString>
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

        protected override MetaDataStreamString ParseTarget()
        {
            return new MetaDataStreamString(PeFile, Offset, _size);
        }
    }
}