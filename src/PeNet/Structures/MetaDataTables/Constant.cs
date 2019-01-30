using PeNet.Test.Structures;

namespace PeNet.Structures.MetaDataTables
{
    public class Constant : AbstractTable
    {
        public Constant(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Type = (byte) ReadSize(1, ref CurrentOffset);
            CurrentOffset += 1; // Padding after "Type"
            Parent = ReadSize(IndexSizes[Index.HasConstant], ref CurrentOffset);
            Value = ReadSize(HeapSizes.Blob, ref CurrentOffset);
        }

        public byte Type {get;}
        public uint Parent {get;}
        public uint Value {get;}
    }
}
