using PeNet.Test.Structures;

namespace PeNet.Structures.MetaDataTables
{
    public class TypeRef : AbstractTable
    {
        public TypeRef(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            ResolutionScope = ReadSize(IndexSizes[Index.ResolutionScope], ref CurrentOffset);
            TypeName = ReadSize(HeapSizes.String, ref CurrentOffset);
            TypeNamespace = ReadSize(HeapSizes.String, ref CurrentOffset);
        }

        public uint ResolutionScope {get;}

        public uint TypeName {get;}

        public uint TypeNamespace {get;}
    }
}
