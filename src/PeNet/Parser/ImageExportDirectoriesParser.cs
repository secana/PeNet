using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageExportDirectoriesParser : SafeParser<IMAGE_EXPORT_DIRECTORY>
    {
        public ImageExportDirectoriesParser(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }


        protected override IMAGE_EXPORT_DIRECTORY ParseTarget()
        {
            return new IMAGE_EXPORT_DIRECTORY(PeFile, Offset);
        }
    }
}