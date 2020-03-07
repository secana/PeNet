using PeNet.Structures;

namespace PeNet.Parser
{
    internal class MetaDataStreamTablesHeaderParser : SafeParser<METADATATABLESHDR>
    {
        public MetaDataStreamTablesHeaderParser(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        protected override METADATATABLESHDR ParseTarget()
        {
            return new METADATATABLESHDR(PeFile, Offset);
        }
    }
}