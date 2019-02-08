namespace PeNet.Structures.MetaDataTables
{
    public class Constant : AbstractTable
    {
        public Constant(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Type = (byte) ReadSize(1);
            CurrentOffset += 1; // Padding after "Type"
            Parent = ReadSize(IndexSizes[Index.HasConstant]);
            Value = ReadSize(HeapSizes.Blob);
        }

        public byte Type {get;}
        public uint Parent {get;}
        public uint Value {get;}
    }
}
