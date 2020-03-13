using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
{
    internal class ImageExportDirectoriesParser : SafeParser<ImageExportDirectory>
    {
        public ImageExportDirectoriesParser(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }


        protected override ImageExportDirectory ParseTarget()
        {
            return new ImageExportDirectory(PeFile, Offset);
        }
    }
}