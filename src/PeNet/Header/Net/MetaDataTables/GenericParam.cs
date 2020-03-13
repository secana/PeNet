using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class GenericParam : AbstractTable
    {
        public GenericParam(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Number = (ushort) ReadSize(2);
            Flags = (ushort) ReadSize(2);
            Owner = ReadSize(IndexSizes[Index.TypeOrMethodDef]);
            Name = ReadSize(HeapSizes.String);
        }

        public ushort Number {get;}
        public ushort Flags {get;}
        public uint Owner {get;}
        public uint Name {get;}
    }
}
