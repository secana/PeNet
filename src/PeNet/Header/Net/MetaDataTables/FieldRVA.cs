using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class FieldRVA : AbstractTable
    {
        public FieldRVA(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            RVA = ReadSize(4);
            Field = ReadSize(IndexSizes[Index.Field]);
        }

        public uint RVA {get;}
        public uint Field {get;}
    }
}
