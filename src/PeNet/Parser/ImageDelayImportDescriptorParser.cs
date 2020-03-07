using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageDelayImportDescriptorParser : SafeParser<IMAGE_DELAY_IMPORT_DESCRIPTOR>
    {
        internal ImageDelayImportDescriptorParser(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        protected override IMAGE_DELAY_IMPORT_DESCRIPTOR ParseTarget()
        {
            return new IMAGE_DELAY_IMPORT_DESCRIPTOR(PeFile, Offset);
        }
    }
}