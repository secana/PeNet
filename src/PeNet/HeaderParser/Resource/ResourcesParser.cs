using PeNet.FileParser;
using PeNet.Header.Resource;

namespace PeNet.HeaderParser.Resource
{
    internal class ResourcesParser : SafeParser<Resources>
    {
        private readonly ResourceLocation _vsVersionLocation;
        private readonly ResourceLocation[] _iconDirectoryLocations;
        private readonly ResourceLocation[] _groupIconDirectoryLocations;

        public ResourcesParser(IRawFile peFile, uint offset, ResourceLocation vsVersionLocation, ResourceLocation[] iconDirectoryLocations, ResourceLocation[] groupIconDirectoryLocations) : base(peFile, offset)
        {
            _vsVersionLocation = vsVersionLocation;
            _iconDirectoryLocations = iconDirectoryLocations;
            _groupIconDirectoryLocations = groupIconDirectoryLocations;
        }

        protected override Resources ParseTarget()
        {
            return new Resources(PeFile, Offset, _vsVersionLocation, _iconDirectoryLocations, _groupIconDirectoryLocations);
        }
    }
}
