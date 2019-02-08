namespace PeNet.Structures.MetaDataTables
{
    public class FieldLayout : AbstractTable
    {
        public FieldLayout(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Offset = ReadSize(4);
            Field = ReadSize(IndexSizes[Index.Field]);
        }

        public new uint Offset {get;}
        public uint Field {get;}
    }
}
