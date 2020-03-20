using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class AssemblyProcessor : AbstractTable
    {
        public AssemblyProcessor(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Processor = ReadSize(4);
        }

        public uint Processor {get;}
    }
}
