using System;

namespace PeNet.Parser
{
    internal class MetaDataStreamBlobParser : SafeParser<byte[]>
    {
        private readonly uint _size;

        public MetaDataStreamBlobParser(
            IRawFile peFile, 
            long offset, 
            uint size) 
            : base(peFile, offset)
        {
            _size = size;
        }

        protected override byte[] ParseTarget() => PeFile.GetSpan(Offset, _size).ToArray();
    }
}