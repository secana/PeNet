using System;

namespace PeNet.Parser
{
    internal class MetaDataStreamBlobParser : SafeParser<byte[]>
    {
        private readonly uint _size;

        public MetaDataStreamBlobParser(
            byte[] buff, 
            uint offset, 
            uint size) 
            : base(buff, offset)
        {
            _size = size;
        }

        protected override byte[] ParseTarget()
        {
            var blob = new byte[_size];
            Array.Copy(_buff, _offset, blob, 0, _size);

            return blob;
        }
    }
}