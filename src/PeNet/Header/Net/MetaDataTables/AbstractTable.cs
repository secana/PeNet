using System;
using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class AbstractTable : AbstractStructure
    {
        protected HeapSizes HeapSizes {get;}
        protected IndexSize IndexSizes {get;}
        protected long CurrentOffset;

        public AbstractTable(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset)
        {
            HeapSizes = heapSizes;
            IndexSizes = indexSizes;
            CurrentOffset = Offset;
        }

        private uint ReadSize(uint size, ref long offset)
        {
            switch(size)
            {
                case 1: 
                    offset += 1;
                    return PeFile.ReadByte(offset - 1);
                case 2:
                    offset += 2;
                    return PeFile.ReadUShort(offset - 2);
                case 4:
                    offset += 4;
                    return PeFile.ReadUInt(offset - 4);
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
