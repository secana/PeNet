using System;
using System.Collections.Generic;
using PeNet.Structures;
using PeNet.Utilities;

namespace PeNet.Parser
{
    internal class MetaDataStreamStringParser : SafeParser<METADATASTREAM_STRING>
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

        protected override METADATASTREAM_STRING ParseTarget()
        {
            return new METADATASTREAM_STRING(_buff, _offset, _size);
        }
    }
}