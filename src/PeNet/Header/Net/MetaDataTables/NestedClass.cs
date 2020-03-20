using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class NestedClass : AbstractTable
    {
        public NestedClass(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            NestedClassType = ReadSize(IndexSizes[Index.TypeDef]);
            EnclosingClassType = ReadSize(IndexSizes[Index.TypeDef]);
        }

        public uint NestedClassType {get;}
        public uint EnclosingClassType {get;}
    }
}
