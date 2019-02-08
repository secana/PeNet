namespace PeNet.Structures.MetaDataTables
{
    public class File : AbstractTable
    {
        public File(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Flags = ReadSize(4);
            Name = ReadSize(HeapSizes.String);
            HashValue = ReadSize(HeapSizes.Blob);
        }

        public uint Flags {get;}
        public uint Name {get;}
        public uint HashValue {get;}
    }
}
