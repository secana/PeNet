namespace PeNet.Structures.MetaDataTables
{
    public class MethodSemantics : AbstractTable
    {
        public MethodSemantics(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
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
