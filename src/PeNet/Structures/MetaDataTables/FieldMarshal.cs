namespace PeNet.Structures.MetaDataTables
{
    public class FieldMarshal : AbstractTable
    {
        public FieldMarshal(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Parent = ReadSize(IndexSizes[Index.HasFieldMarshal]);
            NativeType = ReadSize(HeapSizes.Blob);
        }

        public uint Parent {get;}
        public uint NativeType {get;}
    }
}
