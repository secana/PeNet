using PeNet.FileParser;
using PeNet.Structures;

namespace PeNet.Parser
{
    internal class MetaDataStreamTablesHeaderParser : SafeParser<MetaDataTablesHdr>
    {
        public MetaDataStreamTablesHeaderParser(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        protected override MetaDataTablesHdr ParseTarget()
        {
            return new MetaDataTablesHdr(PeFile, Offset);
        }
    }
}