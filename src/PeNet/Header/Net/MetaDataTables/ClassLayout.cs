using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class ClassLayout : AbstractTable
    {
        public ClassLayout(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            PackingSize = (ushort) ReadSize(2);
            ClassSize = ReadSize(4);
            Parent = ReadSize(IndexSizes[Index.TypeDef]);
        }

        public ushort PackingSize {get;}
        public uint ClassSize {get;}
        public uint Parent {get;}
    }
}
