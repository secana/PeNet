namespace PeNet.Structures.MetaDataTables
{
    public class InterfaceImpl : AbstractTable
    {
        public InterfaceImpl(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Class = ReadSize(IndexSizes[Index.TypeDef]);
            Interface = ReadSize(IndexSizes[Index.TypeDefOrRef]);
        }

        public uint Class {get;}
        public uint Interface {get;}
    }
}
