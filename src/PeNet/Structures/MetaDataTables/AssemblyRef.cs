namespace PeNet.Structures.MetaDataTables
{
    public class AssemblyRef : AbstractTable
    {
        public AssemblyRef(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            MajorVersion = (ushort) ReadSize(2);
            MinorVersion = (ushort) ReadSize(2);
            BuildNumber = (ushort) ReadSize(2);
            RevisionNumber = (ushort) ReadSize(2);
            Flags = ReadSize(4);
            PublicKeyOrToken = ReadSize(HeapSizes.Blob);
            Name = ReadSize(HeapSizes.String);
            Culture = ReadSize(HeapSizes.String);
            HashValue = ReadSize(HeapSizes.Blob);
        }

        public ushort MajorVersion {get;}
        public ushort MinorVersion {get;}
        public ushort BuildNumber {get;}
        public ushort RevisionNumber {get;}
        public uint Flags {get;}
        public uint PublicKeyOrToken {get;}
        public uint Name {get;}
        public uint Culture {get;}
        public uint HashValue {get;}
    }
}
