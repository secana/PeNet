using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class GenericParamConstraint : AbstractTable
    {
        public GenericParamConstraint(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Owner = ReadSize(IndexSizes[Index.GenericParam]);
        }

        public uint Owner  {get;}
        public uint Constraint {get;}
    }
}
