using System;
using System.Collections.Generic;
using PeNet.Utilities;

namespace PeNet.Parser
{
    internal class MetaDataStreamStringParser : SafeParser<List<string>>
    {
        private readonly uint _size;

        public MetaDataStreamStringParser(
            byte[] buff, 
            uint offset,
            uint size
            ) 
            : base(buff, offset)
        {
            _size = size;
        }

        protected override List<string> ParseTarget()
        {
            var stringList = new List<string>();

            for (var i = _offset; i < _offset + _size; i++)
            {
                var tmpString = _buff.GetCString(i);
                i += (uint) tmpString.Length;

                if(String.IsNullOrWhiteSpace(tmpString))
                    continue;

                stringList.Add(tmpString);
            }

            return stringList;
        }
    }
}