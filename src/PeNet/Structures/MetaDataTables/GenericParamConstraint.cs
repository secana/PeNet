namespace PeNet.Structures.MetaDataTables
{
    public class GenericParamConstraint : AbstractTable
    {
        public GenericParamConstraint(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Owner = ReadSize(IndexSizes[Index.GenericParam]);
        }

        public uint Owner  {get;}
        public uint Constraint {get;}
    }
}
