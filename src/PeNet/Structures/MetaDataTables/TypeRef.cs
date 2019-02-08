namespace PeNet.Structures.MetaDataTables
{
    public class TypeRef : AbstractTable
    {
        public TypeRef(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
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
