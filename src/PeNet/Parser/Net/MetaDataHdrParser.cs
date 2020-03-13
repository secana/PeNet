using PeNet.FileParser;
using PeNet.Header.Net;

namespace PeNet.Parser.Net
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