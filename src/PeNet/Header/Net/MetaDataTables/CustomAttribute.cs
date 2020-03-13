using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class CustomAttribute : AbstractTable
    {
        public CustomAttribute(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
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
