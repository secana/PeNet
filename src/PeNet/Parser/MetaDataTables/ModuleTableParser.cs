using PeNet.Structures;
using PeNet.Structures.MetaDataTables;
using PeNet.Structures.MetaDataTables.Indices;
using PeNet.Utilities;

namespace PeNet.Parser.MetaDataTables
{
    internal class ModuleTableParser : SafeParser<ModuleTable>
    {
        private readonly uint _numOfRows;
        private readonly HeapOffsetSizes _heapOffsetSizes;
        private readonly IMETADATASTREAM_STRING _metaDataStreamString;
        private readonly IMETADATASTREAM_GUID _metaDataStreamGuid;

        public ModuleTableParser(
            byte[] buff, 
            uint offset, 
            uint numOfRows,
            IMETADATASTREAM_STRING metaDataStreamString, 
            IMETADATASTREAM_GUID metaDataStreamGuid,
            HeapOffsetSizes heapOffsetSizes
            ) 
            : base(buff, offset)
        {
            _numOfRows = numOfRows;
            _metaDataStreamString = metaDataStreamString;
            _metaDataStreamGuid = metaDataStreamGuid;
            _heapOffsetSizes = heapOffsetSizes;
        }

        protected override ModuleTable ParseTarget()
        {
            return new ModuleTable(_buff, _offset, _numOfRows, _metaDataStreamString, _metaDataStreamGuid, _heapOffsetSizes);
        }
    }
}