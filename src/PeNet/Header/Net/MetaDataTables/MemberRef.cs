using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class MemberRef : AbstractTable
    {
        public MemberRef(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Class = ReadSize(IndexSizes[Index.MemberRefParent]);
            Name = ReadSize(HeapSizes.String);
            Signature = ReadSize(HeapSizes.Blob);
        }

        public uint Class {get;}
        public uint Name  {get;}
        public uint Signature {get;}
    }
}
