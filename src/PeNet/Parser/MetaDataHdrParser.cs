using PeNet.FileParser;
using PeNet.Structures;

namespace PeNet.Parser
{
    internal class MetaDataHdrParser : SafeParser<METADATAHDR>
    {
        public MetaDataHdrParser(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        protected override METADATAHDR ParseTarget()
        {
            return new METADATAHDR(PeFile, Offset);
        }
    }
}