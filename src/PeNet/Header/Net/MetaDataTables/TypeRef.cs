using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class TypeRef : AbstractTable
    {
        public TypeRef(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            ResolutionScope = ReadSize(IndexSizes[Index.ResolutionScope]);
            TypeName = ReadSize(HeapSizes.String);
            TypeNamespace = ReadSize(HeapSizes.String);
        }

        public uint ResolutionScope {get;}

        public uint TypeName {get;}

        public uint TypeNamespace {get;}
    }
}
