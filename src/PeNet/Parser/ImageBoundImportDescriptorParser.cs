using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageBoundImportDescriptorParser : SafeParser<IMAGE_BOUND_IMPORT_DESCRIPTOR>
    {
        internal ImageBoundImportDescriptorParser(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        protected override IMAGE_BOUND_IMPORT_DESCRIPTOR ParseTarget()
        {
            return new IMAGE_BOUND_IMPORT_DESCRIPTOR(_buff, _offset);
        }
    }
}