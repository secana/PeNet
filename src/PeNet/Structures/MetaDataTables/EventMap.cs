namespace PeNet.Structures.MetaDataTables
{
    public class EventMap : AbstractTable
    {
        public EventMap(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Parent = ReadSize(IndexSizes[Index.TypeDef]);
            EventList = ReadSize(IndexSizes[Index.Event]);
        }

        public uint Parent {get;}
        public uint EventList {get;}
    }
}
