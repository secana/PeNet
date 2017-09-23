using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageNtHeadersParser : SafeParser<IMAGE_NT_HEADERS>
    {
        private readonly bool _is64Bit;

        internal ImageNtHeadersParser(byte[] buff, uint offset, bool is64Bit)
            : base(buff, offset)
        {
            _is64Bit = is64Bit;
        }

        protected override IMAGE_NT_HEADERS ParseTarget()
        {
            return new IMAGE_NT_HEADERS(_buff, _offset, _is64Bit);
        }
    }
}