using PeNet.FileParser;
using PeNet.Structures;

namespace PeNet.Parser
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