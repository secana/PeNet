using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class MethodSemantics : AbstractTable
    {
        public MethodSemantics(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Semantics = (ushort) ReadSize(2);
            Method = ReadSize(IndexSizes[Index.MethodDef]);
            Association = ReadSize(IndexSizes[Index.HasSemantics]);
        }

        public ushort Semantics {get;}
        public uint Method  {get;}
        public uint Association {get;}
    }
}
