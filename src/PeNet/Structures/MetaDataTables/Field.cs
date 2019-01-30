using PeNet.Test.Structures;

namespace PeNet.Structures.MetaDataTables
{
    public class Field : AbstractTable
    {
        public Field(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Flags = (ushort) ReadSize(2, ref CurrentOffset);
            Name = ReadSize(HeapSizes.String, ref CurrentOffset);
            Signature = ReadSize(HeapSizes.Blob, ref CurrentOffset);
        }

        public ushort Flags {get;}
        
        public uint Name {get;}

        public uint Signature {get;}
    }
}
