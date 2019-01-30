using PeNet.Test.Structures;

namespace PeNet.Structures.MetaDataTables
{
    public class MemberRef : AbstractTable
    {
        public MemberRef(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Class = ReadSize(IndexSizes[Index.MemberRefParent], ref CurrentOffset);
            Name = ReadSize(HeapSizes.String, ref CurrentOffset);
            Signature = ReadSize(HeapSizes.Blob, ref CurrentOffset);
        }

        public uint Class {get;}
        public uint Name  {get;}
        public uint Signature {get;}
    }
}
