using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageDebugDirectoryParser : SafeParser<IMAGE_DEBUG_DIRECTORY>
    {
        internal ImageDebugDirectoryParser(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        protected override IMAGE_DEBUG_DIRECTORY ParseTarget()
        {
            return new IMAGE_DEBUG_DIRECTORY(_buff, _offset);
        }
    }
}