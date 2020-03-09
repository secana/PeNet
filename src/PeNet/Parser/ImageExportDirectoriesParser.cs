using PeNet.Structures;

namespace PeNet.Parser
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