using PeNet.Structures;
using System.IO;

namespace PeNet.Parser
{
    internal class ImageNtHeadersParser : SafeParser<IMAGE_NT_HEADERS>
    {
        internal ImageNtHeadersParser(Stream peFile, uint offset)
            : base(peFile, offset)
        {
        }

        protected override IMAGE_NT_HEADERS ParseTarget()
        {
            return new IMAGE_NT_HEADERS(PeFile, Offset);
        }
    }
}