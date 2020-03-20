using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class Assembly : AbstractTable
    {
        public Assembly(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            HashAlgId = ReadSize(4);
            MajorVersion = (ushort) ReadSize(2);
            MinorVersion = (ushort) ReadSize(2);
            BuildNumber = (ushort) ReadSize(2);
            RevisionNumber = (ushort) ReadSize(2);
            Flags = ReadSize(4);
            PublicKey = ReadSize(HeapSizes.Blob);
            Name = ReadSize(HeapSizes.String);
            Culture = ReadSize(HeapSizes.String);
        }

        public uint HashAlgId {get;}
        public ushort MajorVersion {get;}
        public ushort MinorVersion {get;}
        public ushort BuildNumber {get;}
        public ushort RevisionNumber {get;}
        public uint Flags {get;}
        public uint PublicKey {get;}
        public uint Name {get;}
        public uint Culture {get;}
    }
}
