namespace PeNet.Structures.MetaDataTables
{
    public class TypeSpec : AbstractTable
    {
        public TypeSpec(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Signature = ReadSize(HeapSizes.Blob);
        }

        public uint Signature {get;}
    }
}
