using PeNet.Test.Structures;
using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    public class Module : AbstractTable
    {
        public Module(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {

            Generation = (ushort) ReadSize(2, ref CurrentOffset);
            Name = ReadSize(HeapSizes.String, ref CurrentOffset);
            Mvid = ReadSize(HeapSizes.Guid, ref CurrentOffset);
            EncId = ReadSize(HeapSizes.Guid, ref CurrentOffset);
            EncBaseId = ReadSize(HeapSizes.Guid, ref CurrentOffset);
        }

        public ushort Generation {get;}

        public uint Name {get;}

        public uint Mvid {get;}

        public uint EncId {get;}

        public uint EncBaseId {get;}
    }
}
