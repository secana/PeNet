namespace PeNet.Structures.MetaDataTables
{
    public class MemberRef : AbstractTable
    {
        public MemberRef(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
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
