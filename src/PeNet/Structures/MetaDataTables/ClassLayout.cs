namespace PeNet.Structures.MetaDataTables
{
    public class ClassLayout : AbstractTable
    {
        public ClassLayout(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
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
