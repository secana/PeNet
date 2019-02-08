namespace PeNet.Structures.MetaDataTables
{
    public class Event : AbstractTable
    {
        public Event(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            EventFlags = (ushort) ReadSize(2);
            Name = ReadSize(HeapSizes.String);
            EventType = ReadSize(IndexSizes[Index.TypeDefOrRef]);
        }

        public ushort EventFlags {get;}
        public uint Name {get;}
        public uint EventType {get;}
    }
}
