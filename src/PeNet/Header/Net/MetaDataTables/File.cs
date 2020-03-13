using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class File : AbstractTable
    {
        public File(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Flags = ReadSize(4);
            Name = ReadSize(HeapSizes.String);
            HashValue = ReadSize(HeapSizes.Blob);
        }

        public uint Flags {get;}
        public uint Name {get;}
        public uint HashValue {get;}
    }
}
