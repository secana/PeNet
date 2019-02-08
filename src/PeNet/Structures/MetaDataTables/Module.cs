namespace PeNet.Structures.MetaDataTables
{
    public class Module : AbstractTable
    {
        public Module(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {

            Generation = (ushort) ReadSize(2);
            Name = ReadSize(HeapSizes.String);
            Mvid = ReadSize(HeapSizes.Guid);
            EncId = ReadSize(HeapSizes.Guid);
            EncBaseId = ReadSize(HeapSizes.Guid);
        }

        public ushort Generation {get;}

        public uint Name {get;}

        public uint Mvid {get;}

        public uint EncId {get;}

        public uint EncBaseId {get;}
    }
}
