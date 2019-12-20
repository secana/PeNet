using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageNtHeadersParser : SafeParser<IMAGE_NT_HEADERS>
    {
        internal ImageNtHeadersParser(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        protected override IMAGE_NT_HEADERS ParseTarget()
        {
            return new IMAGE_NT_HEADERS(Buff, Offset);
        }
    }
}