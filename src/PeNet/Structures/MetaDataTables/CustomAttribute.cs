using PeNet.Test.Structures;

namespace PeNet.Structures.MetaDataTables
{
    public class CustomAttribute : AbstractTable
    {
        public CustomAttribute(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Parent = ReadSize(IndexSizes[Index.HasCustomAttribute], ref CurrentOffset);
            Type = ReadSize(IndexSizes[Index.CustomAttributeType], ref CurrentOffset);
            Value = ReadSize(HeapSizes.Blob, ref CurrentOffset);
        }

        public uint Parent {get;}
        public uint Type {get;}
        public uint Value {get;}
    }
}
