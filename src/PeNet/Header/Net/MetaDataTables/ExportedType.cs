using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class ExportedType : AbstractTable
    {
        public ExportedType(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Flags = ReadSize(4);
            TypeDefId = ReadSize(IndexSizes[Index.TypeDef]);
            TypeName = ReadSize(HeapSizes.String);
            TypeNamespace = ReadSize(HeapSizes.String);
        }

        public uint Flags {get;}
        public uint TypeDefId {get;}
        public uint TypeName {get;}
        public uint TypeNamespace {get;}
    }
}
