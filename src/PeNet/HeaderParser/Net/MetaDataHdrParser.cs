using PeNet.FileParser;
using PeNet.Header.Net;

namespace PeNet.HeaderParser.Net
{
    internal class MetaDataHdrParser : SafeParser<MetaDataHdr>
    {
        public MetaDataHdrParser(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        protected override MetaDataHdr ParseTarget()
        {
            return new MetaDataHdr(PeFile, Offset);
        }
    }
}