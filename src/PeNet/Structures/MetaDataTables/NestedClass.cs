namespace PeNet.Structures.MetaDataTables
{
    public class NestedClass : AbstractTable
    {
        public NestedClass(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            NestedClassType = ReadSize(IndexSizes[Index.TypeDef]);
            EnclosingClassType = ReadSize(IndexSizes[Index.TypeDef]);
        }

        public uint NestedClassType {get;}
        public uint EnclosingClassType {get;}
    }
}
