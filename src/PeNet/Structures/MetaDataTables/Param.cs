namespace PeNet.Structures.MetaDataTables
{
    public class Param : AbstractTable
    {
        public Param(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Flags = (ushort) ReadSize(2);
            Sequence = (ushort) ReadSize(2);
            Name = ReadSize(HeapSizes.String);
        }

        public ushort Flags {get;}
        public ushort Sequence {get;}
        public uint Name {get;}
    }
}
