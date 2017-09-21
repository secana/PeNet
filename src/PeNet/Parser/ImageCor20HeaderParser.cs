using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageCor20HeaderParser : SafeParser<IMAGE_COR20_HEADER>
    {
        public ImageCor20HeaderParser(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        protected override IMAGE_COR20_HEADER ParseTarget()
        {
            return new IMAGE_COR20_HEADER(_buff, _offset);
        }
    }
}