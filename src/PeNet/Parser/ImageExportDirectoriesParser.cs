using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageExportDirectoriesParser : SafeParser<IMAGE_EXPORT_DIRECTORY>
    {
        public ImageExportDirectoriesParser(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }


        protected override IMAGE_EXPORT_DIRECTORY ParseTarget()
        {
            return new IMAGE_EXPORT_DIRECTORY(_buff, _offset);
        }
    }
}