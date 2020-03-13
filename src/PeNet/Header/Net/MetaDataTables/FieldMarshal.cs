using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class FieldMarshal : AbstractTable
    {
        public FieldMarshal(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Parent = ReadSize(IndexSizes[Index.HasFieldMarshal]);
            NativeType = ReadSize(HeapSizes.Blob);
        }

        public uint Parent {get;}
        public uint NativeType {get;}
    }
}
