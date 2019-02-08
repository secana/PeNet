namespace PeNet.Structures.MetaDataTables
{
    public class Field : AbstractTable
    {
        public Field(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Flags = (ushort) ReadSize(2);
            Name = ReadSize(HeapSizes.String);
            Signature = ReadSize(HeapSizes.Blob);
        }

        public ushort Flags {get;}
        
        public uint Name {get;}

        public uint Signature {get;}
    }
}
