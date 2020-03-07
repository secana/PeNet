using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageBoundImportDescriptorParser : SafeParser<IMAGE_BOUND_IMPORT_DESCRIPTOR>
    {
        internal ImageBoundImportDescriptorParser(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        protected override IMAGE_BOUND_IMPORT_DESCRIPTOR ParseTarget()
        {
            return new IMAGE_BOUND_IMPORT_DESCRIPTOR(PeFile, Offset);
        }
    }
}