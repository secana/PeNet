using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class MethodImpl : AbstractTable
    {
        public MethodImpl(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
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
