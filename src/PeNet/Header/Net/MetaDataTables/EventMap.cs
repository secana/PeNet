using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class EventMap : AbstractTable
    {
        public EventMap(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Parent = ReadSize(IndexSizes[Index.TypeDef]);
            EventList = ReadSize(IndexSizes[Index.Event]);
        }

        public uint Parent {get;}
        public uint EventList {get;}
    }
}
