using PeNet.FileParser;

namespace PeNet.Parser.Net
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

        protected override byte[] ParseTarget() => PeFile.AsSpan(Offset, _size).ToArray();
    }
}