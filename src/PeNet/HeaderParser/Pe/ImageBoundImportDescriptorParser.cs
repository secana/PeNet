using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
{
    internal class ImageBoundImportDescriptorParser : SafeParser<ImageBoundImportDescriptor>
    {
        internal ImageBoundImportDescriptorParser(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        protected override ImageBoundImportDescriptor ParseTarget()
        {
            return new ImageBoundImportDescriptor(PeFile, Offset);
        }
    }
}