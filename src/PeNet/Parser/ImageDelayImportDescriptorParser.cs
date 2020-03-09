using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageDelayImportDescriptorParser : SafeParser<ImageDelayImportDescriptor>
    {
        internal ImageDelayImportDescriptorParser(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        protected override ImageDelayImportDescriptor ParseTarget()
        {
            return new ImageDelayImportDescriptor(PeFile, Offset);
        }
    }
}