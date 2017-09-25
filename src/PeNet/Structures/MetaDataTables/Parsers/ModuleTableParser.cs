using PeNet.Parser;
using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables.Parsers
{
    internal class ModuleTableParser : SafeParser<ModuleTable>
    {
        private readonly uint _numOfRows;
        private readonly HeapOffsetBasedIndexSizes _heapOffsetSizes;

        public ModuleTableParser(byte[] buff, uint offset, uint numOfRows, HeapOffsetBasedIndexSizes heapOffsetSizes) 
            : base(buff, offset)
        {
            _numOfRows = numOfRows;
            _heapOffsetSizes = heapOffsetSizes;
        }

        protected override ModuleTable ParseTarget()
        {
            return new ModuleTable(_buff, _offset, _numOfRows, _heapOffsetSizes);
        }
    }
}