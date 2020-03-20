using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
{
    internal class ImageNtHeadersParser : SafeParser<ImageNtHeaders>
    {
        internal ImageNtHeadersParser(IRawFile peFile, uint offset)
            : base(peFile, offset)
        {
        }

        protected override ImageNtHeaders ParseTarget()
        {
            return new ImageNtHeaders(PeFile, Offset);
        }
    }
}