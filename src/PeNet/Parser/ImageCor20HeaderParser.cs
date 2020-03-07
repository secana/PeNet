using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageCor20HeaderParser : SafeParser<IMAGE_COR20_HEADER>
    {
        public ImageCor20HeaderParser(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        protected override IMAGE_COR20_HEADER ParseTarget()
        {
            return new IMAGE_COR20_HEADER(PeFile, Offset);
        }
    }
}