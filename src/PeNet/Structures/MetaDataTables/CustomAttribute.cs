namespace PeNet.Structures.MetaDataTables
{
    public class CustomAttribute : AbstractTable
    {
        public CustomAttribute(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Parent = ReadSize(IndexSizes[Index.HasCustomAttribute]);
            Type = ReadSize(IndexSizes[Index.CustomAttributeType]);
            Value = ReadSize(HeapSizes.Blob);
        }

        public uint Parent {get;}
        public uint Type {get;}
        public uint Value {get;}
    }
}
