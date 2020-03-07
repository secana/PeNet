using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageDosHeaderParser : SafeParser<IMAGE_DOS_HEADER>
    {
        internal ImageDosHeaderParser(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        protected override IMAGE_DOS_HEADER ParseTarget()
        {
            return new IMAGE_DOS_HEADER(PeFile, Offset);
        }
    }
}