using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class ManifestResource : AbstractTable
    {
        public ManifestResource(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
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
