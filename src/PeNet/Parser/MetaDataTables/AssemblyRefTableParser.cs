using PeNet.Structures;
using PeNet.Structures.MetaDataTables;
using PeNet.Utilities;

namespace PeNet.Parser.MetaDataTables
{
    internal class AssemblyRefTableParser : SafeParser<AssemblyRefTable>
    {
        private readonly uint _numOfRows;
        private readonly IMETADATASTREAM_STRING _metaDataStreamString;
        private readonly HeapOffsetBasedIndexSizes _heapOffsetSizes;
        private readonly IMETADATASTREAM_BLOB _metaDataStreamBlob;

        public AssemblyRefTableParser(
            byte[] buff, 
            uint offset,
            uint numOfRows,
            HeapOffsetBasedIndexSizes heapOffsetSizes,
            IMETADATASTREAM_STRING metaDataStreamString,
            IMETADATASTREAM_BLOB metadatastreamBlob
            )
            : base(buff, offset)
        {
            _numOfRows = numOfRows;
            _metaDataStreamString = metaDataStreamString;
            _metaDataStreamBlob = metadatastreamBlob;
            _heapOffsetSizes = heapOffsetSizes;
        }

        protected override AssemblyRefTable ParseTarget()
        {
            return new AssemblyRefTable(
                _buff,
                _offset, // TODO: Compute offset of the AssemblyRefTable based on the length of the previous tables
                _numOfRows,
                _heapOffsetSizes,
                _metaDataStreamString,
                _metaDataStreamBlob);
        }
    }
}