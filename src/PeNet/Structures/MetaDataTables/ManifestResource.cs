namespace PeNet.Structures.MetaDataTables
{
    public class ManifestResource : AbstractTable
    {
        public ManifestResource(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Offset = ReadSize(4);
            Flags = ReadSize(4);
            Name = ReadSize(HeapSizes.String);
            Implementation = ReadSize(IndexSizes[Index.Implementation]);
        }

        public new uint Offset {get;}
        public uint Flags {get;}
        public uint Name {get;}
        public uint Implementation  {get;}
    }
}
