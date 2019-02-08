namespace PeNet.Structures.MetaDataTables
{
    public class AssemblyOS : AbstractTable
    {
        public AssemblyOS(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            OSPlatformID = ReadSize(4);
            OSMajorVersion = ReadSize(4);
            OSMinorVersion = ReadSize(4);
        }

        public uint OSPlatformID {get;}
        public uint OSMajorVersion {get;}
        public uint OSMinorVersion {get;}
    }
}
