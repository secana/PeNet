using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class Field : AbstractTable
    {
        public Field(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Flags = (ushort) ReadSize(2);
            Name = ReadSize(HeapSizes.String);
            Signature = ReadSize(HeapSizes.Blob);
        }

        public ushort Flags {get;}
        
        public uint Name {get;}

        public uint Signature {get;}
    }
}
