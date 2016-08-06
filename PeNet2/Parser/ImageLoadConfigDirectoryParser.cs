using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageLoadConfigDirectoryParser : SafeParser<IMAGE_LOAD_CONFIG_DIRECTORY>
    {
        private readonly bool _is64Bit;

        internal ImageLoadConfigDirectoryParser(byte[] buff, uint offset, bool is64Bit) 
            : base(buff, offset)
        {
            _is64Bit = is64Bit;
        }

        protected override IMAGE_LOAD_CONFIG_DIRECTORY ParseTarget()
        {
            return new IMAGE_LOAD_CONFIG_DIRECTORY(_buff, _offset, _is64Bit);
        }
    }
}