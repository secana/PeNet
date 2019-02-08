namespace PeNet.Structures.MetaDataTables
{
    public class MethodImpl : AbstractTable
    {
        public MethodImpl(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Class = ReadSize(IndexSizes[Index.TypeDef]);
            MethodBody = ReadSize(IndexSizes[Index.MethodDefOrRef]);
            MethodDeclaration = ReadSize(IndexSizes[Index.MethodDefOrRef]);
        }

        public uint Class {get;}
        public uint MethodBody {get;}
        public uint MethodDeclaration {get;}
    }
}
