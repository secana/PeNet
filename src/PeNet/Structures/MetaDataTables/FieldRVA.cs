namespace PeNet.Structures.MetaDataTables
{
    public class FieldRVA : AbstractTable
    {
        public FieldRVA(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            RVA = ReadSize(4);
            Field = ReadSize(IndexSizes[Index.Field]);
        }

        public uint RVA {get;}
        public uint Field {get;}
    }
}
