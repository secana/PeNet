using PeNet.FileParser;
using PeNet.Structures;

namespace PeNet.Parser
{
    internal class MetaDataStreamUsParser : SafeParser<MetaDataStreamUs>
    {
        private readonly uint _size;

        public MetaDataStreamUsParser(IRawFile peFile, long offset, uint size) 
            : base(peFile, offset)
        {
            _size = size;
        }

        protected override MetaDataStreamUs ParseTarget()
        {
            return new MetaDataStreamUs(PeFile, Offset, _size);
        }
    }
}