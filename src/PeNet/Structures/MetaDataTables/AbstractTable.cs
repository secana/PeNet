using PeNet.Utilities;
using System;

namespace PeNet.Structures.MetaDataTables
{
    public class AbstractTable : AbstractStructure
    {
        protected HeapSizes HeapSizes {get;}
        protected IndexSize IndexSizes {get;}
        protected uint CurrentOffset;

        public AbstractTable(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset)
        {
            HeapSizes = heapSizes;
            IndexSizes = indexSizes;
            CurrentOffset = Offset;
        }

        private uint ReadSize(uint size, ref uint offset)
        {
            switch(size)
            {
                case 1: 
                    offset += 1;
                    return Buff[offset - 1];
                case 2:
                    offset += 2;
                    return Buff.BytesToUInt16(offset - 2);
                case 4:
                    offset += 4;
                    return Buff.BytesToUInt32(offset - 4);
                default:
                    throw new ArgumentException("Unsupported offset size.");
            }
        }

        protected uint ReadSize(uint size)
        {
            return ReadSize(size, ref CurrentOffset);
        }
    }
}
