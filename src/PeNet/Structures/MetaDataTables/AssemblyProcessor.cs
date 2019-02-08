namespace PeNet.Structures.MetaDataTables
{
    public class AssemblyProcessor : AbstractTable
    {
        public AssemblyProcessor(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Processor = ReadSize(4);
        }

        public uint Processor {get;}
    }
}
