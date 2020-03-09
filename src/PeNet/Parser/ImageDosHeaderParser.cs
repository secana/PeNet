using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageDosHeaderParser : SafeParser<ImageDosHeader>
    {
        internal ImageDosHeaderParser(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        protected override ImageDosHeader ParseTarget()
        {
            return new ImageDosHeader(PeFile, Offset);
        }
    }
}