using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class InterfaceImpl : AbstractTable
    {
        public InterfaceImpl(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Class = ReadSize(IndexSizes[Index.TypeDef]);
            Interface = ReadSize(IndexSizes[Index.TypeDefOrRef]);
        }

        public uint Class {get;}
        public uint Interface {get;}
    }
}
