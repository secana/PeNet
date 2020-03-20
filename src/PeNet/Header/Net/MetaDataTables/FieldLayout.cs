using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class FieldLayout : AbstractTable
    {
        public FieldLayout(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Offset = ReadSize(4);
            Field = ReadSize(IndexSizes[Index.Field]);
        }

        public new uint Offset {get;}
        public uint Field {get;}
    }
}
