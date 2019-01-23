using System;
using PeNet.Test.Structures;
using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    public class Module : AbstractStructure
    {
        private readonly HeapSizes _heapSizes;
        private readonly IndexSize _indexSizes;

        public Module(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset)
        {
            _heapSizes = heapSizes;
            _indexSizes = indexSizes;
            var currentOffset = offset;

            Generation = buff.BytesToUInt16(Offset);
            Name = ReadSize(_heapSizes.String, ref currentOffset);
            Mvid = ReadSize(_heapSizes.Guid, ref currentOffset);
            EncId = ReadSize(_heapSizes.Guid, ref currentOffset);
            EncBaseId = ReadSize(_heapSizes.Guid, ref currentOffset);
        }

        public ushort Generation {get;}

        public uint Name {get;}

        public uint Mvid {get;}

        public uint EncId {get;}

        public uint EncBaseId {get;}

        private uint ReadSize(uint size, ref uint offset)
        {
            if(size == 2)
            {
                offset += 2;
                return Buff.BytesToUInt16(offset);
            }
            
            offset += 4;
            return Buff.BytesToUInt32(offset);
        }
    }
}
