using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class PropertyMap : AbstractTable
    {
        public PropertyMap(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Parent = ReadSize(IndexSizes[Index.TypeDef]);
            PropertyList = ReadSize(IndexSizes[Index.Property]);
        }

        public uint Parent {get;}
        public uint PropertyList {get;}
    }
}
