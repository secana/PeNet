using PeNet.Test.Structures;

namespace PeNet.Structures.MetaDataTables
{
    public class InterfaceImpl : AbstractTable
    {
        public InterfaceImpl(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Class = ReadSize(IndexSizes[Index.TypeDef], ref CurrentOffset);
            Interface = ReadSize(IndexSizes[Index.TypeDefOrRef], ref CurrentOffset);
        }

        public uint Class {get;}
        public uint Interface {get;}
    }
}
