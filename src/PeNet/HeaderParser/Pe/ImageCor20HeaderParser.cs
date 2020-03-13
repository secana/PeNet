using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
{
    internal class ImageCor20HeaderParser : SafeParser<ImageCor20Header>
    {
        public ImageCor20HeaderParser(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        protected override ImageCor20Header ParseTarget()
        {
            return new ImageCor20Header(PeFile, Offset);
        }
    }
}