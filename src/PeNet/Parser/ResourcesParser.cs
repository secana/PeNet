using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ResourcesParser : SafeParser<Resources>
    {
        private readonly uint _vsVersionOffset;

        public ResourcesParser(byte[] buff, uint offset, uint vsVersionOffset) : base(buff, offset)
        {
            _vsVersionOffset = vsVersionOffset;
        }

        protected override Resources ParseTarget()
        {
            return new Resources(Buff, Offset, _vsVersionOffset);
        }
    }
}