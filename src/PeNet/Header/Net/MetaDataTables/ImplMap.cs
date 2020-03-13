using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class ImplMap : AbstractTable
    {
        public ImplMap(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
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
