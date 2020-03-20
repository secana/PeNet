using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class ModuleRef : AbstractTable
    {
        public ModuleRef(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Name = ReadSize(HeapSizes.String);
        }

        public uint Name {get;}
    }
}
