namespace PeNet.Structures.MetaDataTables
{
    public class Property : AbstractTable
    {
        public Property(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Flags = (ushort) ReadSize(2);
            Name = ReadSize(HeapSizes.String);
            Type = ReadSize(HeapSizes.Blob);
        }

        public ushort Flags {get;}
        public uint Name  {get;}
        public uint Type {get;}
    }
}
