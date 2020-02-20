using PeNet.Structures;
using System.IO;

namespace PeNet.Parser
{
    internal class ImageDosHeaderParser : SafeParser<IMAGE_DOS_HEADER>
    {
        internal ImageDosHeaderParser(Stream peFile, uint offset)
            : base(peFile, offset)
        {
        }

        protected override IMAGE_DOS_HEADER ParseTarget()
        {
            return new IMAGE_DOS_HEADER(PeFile, Offset);
        }
    }
}