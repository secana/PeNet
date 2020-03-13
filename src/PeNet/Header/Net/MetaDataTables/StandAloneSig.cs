using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class StandAloneSig : AbstractTable
    {
        public StandAloneSig(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Signature = ReadSize(HeapSizes.Blob);
        }

        public uint Signature {get;}
    }
}
