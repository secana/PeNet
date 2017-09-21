using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageDelayImportDescriptorParser : SafeParser<IMAGE_DELAY_IMPORT_DESCRIPTOR>
    {
        internal ImageDelayImportDescriptorParser(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        protected override IMAGE_DELAY_IMPORT_DESCRIPTOR ParseTarget()
        {
            return new IMAGE_DELAY_IMPORT_DESCRIPTOR(_buff, _offset);
        }
    }
}