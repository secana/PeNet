namespace PeNet.Structures.MetaDataTables
{
    public class ImplMap : AbstractTable
    {
        public ImplMap(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            MappingFlags = (ushort) ReadSize(2);
            MemberForwarded = ReadSize(IndexSizes[Index.MemberForwarded]);
            ImportName = ReadSize(HeapSizes.String);
            ImportScope = ReadSize(IndexSizes[Index.ModuleRef]);
        }

        public ushort MappingFlags {get;}
        public uint MemberForwarded {get;}
        public uint ImportName {get;}
        public uint ImportScope {get;}
    }
}
