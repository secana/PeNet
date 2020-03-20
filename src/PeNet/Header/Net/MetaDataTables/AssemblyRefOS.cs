using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class AssemblyRefOS : AbstractTable
    {
        public AssemblyRefOS(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            OSPlatformID = ReadSize(4);
            OSMajorVersion = ReadSize(4);
            OSMinorVersion = ReadSize(4);
            AssemblyRef = ReadSize(IndexSizes[Index.AssemblyRef]);
        }

        public uint OSPlatformID {get;}
        public uint OSMajorVersion {get;}
        public uint OSMinorVersion {get;}
        public uint AssemblyRef {get;}
    }
}
