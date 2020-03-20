using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
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