using System;
using System.Linq;
using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables.Indices
{
    public class AbstractIndex
    {
        public int IndexSize { get; private set; }
        public int Index { get; private set; }
        public int Tag { get; private set; }

        protected byte[] _buff { get; }
        protected uint _offset { get; }

        protected AbstractIndex(
            byte[] buff,
            uint offset,
            int numOfTags,
            params int[] numRowsTable)
        {
            _buff = buff;
            _offset = offset;

            ComputeIndexValues(numOfTags, numRowsTable.Max());
        }

        private void ComputeIndexValues(int numOfTags, int maxTableLength)
        {
            var bitsForTags   = (int) Math.Ceiling(Math.Log(numOfTags, 2));
            var bitsForIndex  = 16 - bitsForTags;
            var maxRowsInWord = Math.Pow(2, bitsForIndex) - 1;

            IndexSize = maxTableLength > maxRowsInWord ? 4 : 2;
            var value = IndexSize == 4 ? _buff.BytesToInt32(_offset) : _buff.BytesToUInt16(_offset);
            Index = value >> bitsForTags;
            Tag = value & (int)(Math.Ceiling(Math.Pow(2, bitsForTags)) - 1);
        }
    }
}