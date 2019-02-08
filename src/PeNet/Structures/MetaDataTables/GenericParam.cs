namespace PeNet.Structures.MetaDataTables
{
    public class GenericParam : AbstractTable
    {
        public GenericParam(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Number = (ushort) ReadSize(2);
            Flags = (ushort) ReadSize(2);
            Owner = ReadSize(IndexSizes[Index.TypeOrMethodDef]);
            Name = ReadSize(HeapSizes.String);
        }

        public ushort Number {get;}
        public ushort Flags {get;}
        public uint Owner {get;}
        public uint Name {get;}
    }
}
