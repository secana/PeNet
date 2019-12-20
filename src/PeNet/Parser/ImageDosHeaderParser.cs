using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageDosHeaderParser : SafeParser<IMAGE_DOS_HEADER>
    {
        internal ImageDosHeaderParser(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        protected override IMAGE_DOS_HEADER ParseTarget()
        {
            return new IMAGE_DOS_HEADER(Buff, Offset);
        }
    }
}