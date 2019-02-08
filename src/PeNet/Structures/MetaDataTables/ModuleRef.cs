namespace PeNet.Structures.MetaDataTables
{
    public class ModuleRef : AbstractTable
    {
        public ModuleRef(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Name = ReadSize(HeapSizes.String);
        }

        public uint Name {get;}
    }
}
