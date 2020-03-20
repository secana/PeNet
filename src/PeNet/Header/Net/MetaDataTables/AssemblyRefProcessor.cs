using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class AssemblyRefProcessor : AbstractTable
    {
        public AssemblyRefProcessor(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Processor = ReadSize(4);
            AssemblyRef = ReadSize(IndexSizes[Index.AssemblyRef]);
        }

        public uint Processor {get;}
        public uint AssemblyRef {get;}
    }
}
