using PeNet.Test.Structures;

namespace PeNet.Structures.MetaDataTables
{
    public class Param : AbstractTable
    {
        public Param(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Flags = (ushort) ReadSize(2, ref CurrentOffset);
            Sequence = (ushort) ReadSize(2, ref CurrentOffset);
            Name = ReadSize(HeapSizes.String, ref CurrentOffset);
        }

        public ushort Flags {get;}
        public ushort Sequence {get;}
        public uint Name {get;}
    }
}
