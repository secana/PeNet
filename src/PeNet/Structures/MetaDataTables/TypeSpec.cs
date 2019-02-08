namespace PeNet.Structures.MetaDataTables
{
    public class TypeSpec : AbstractTable
    {
        public TypeSpec(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Signature = ReadSize(HeapSizes.Blob);
        }

        public uint Signature {get;}
    }
}
