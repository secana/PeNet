namespace PeNet.Structures.MetaDataTables
{
    public class AssemblyRefOS : AbstractTable
    {
        public AssemblyRefOS(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
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
