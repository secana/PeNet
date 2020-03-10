using PeNet.FileParser;
using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ResourcesParser : SafeParser<Resources>
    {
        private readonly uint _vsVersionOffset;

        public ResourcesParser(IRawFile peFile, uint offset, uint vsVersionOffset) : base(peFile, offset)
        {
            _vsVersionOffset = vsVersionOffset;
        }

        protected override Resources ParseTarget()
        {
            return new Resources(PeFile, Offset, _vsVersionOffset);
        }
    }
}