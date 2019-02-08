namespace PeNet.Structures.MetaDataTables
{
    public class PropertyMap : AbstractTable
    {
        public PropertyMap(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Parent = ReadSize(IndexSizes[Index.TypeDef]);
            PropertyList = ReadSize(IndexSizes[Index.Property]);
        }

        public uint Parent {get;}
        public uint PropertyList {get;}
    }
}
