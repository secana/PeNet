using PeNet.Structures;

namespace PeNet.Parser
{
    internal class MetaDataHdrParser : SafeParser<METADATAHDR>
    {
        public MetaDataHdrParser(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        protected override METADATAHDR ParseTarget()
        {
            return new METADATAHDR(PeFile, Offset);
        }
    }
}