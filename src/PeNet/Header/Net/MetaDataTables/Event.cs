using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class Event : AbstractTable
    {
        public Event(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
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
