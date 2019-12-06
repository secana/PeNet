using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ResourcesParser : SafeParser<Resources>
    {
        public ResourcesParser(byte[] buff, uint offset) : base(buff, offset)
        {
        }

        protected override Resources ParseTarget()
        {
            return new Resources(_buff, _offset);
        }
    }
}