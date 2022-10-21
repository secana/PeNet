using System.Linq;
using PeNet.FileParser;

namespace PeNet.Header.Resource
{
    /// <summary>
    ///     Parsed version of the Resource data directory.
    /// </summary>
    public class Resources : AbstractStructure
    {
        private bool _vsVersionInfoParsed;
        private VsVersionInfo? _vsVersionInfo;
        private readonly ResourceLocation _vsVersionLocation;

        private bool _iconsParsed;
        private Icon[]? _icons;
        private readonly ResourceLocation[] _iconDirectoryLocations;

        private bool _groupIconDirectoriesParsed;
        private GroupIconDirectory[]? _groupIconDirectories;
        private readonly ResourceLocation[] _groupIconDirectoryLocations;

        /// <summary>
        ///     Optional Resource member which contains version information
        ///     and compile time information about the binary.
        /// </summary>
        public VsVersionInfo? VsVersionInfo
        {
            get
            {
                if (_vsVersionInfoParsed) return _vsVersionInfo;
                _vsVersionInfoParsed = true;
                return _vsVersionInfo ??= new VsVersionInfo(PeFile, _vsVersionLocation.Offset);
            }
        }

        /// <summary>
        ///     Optional Resource member which contains icon information
        ///     and icon image as bytes.
        /// </summary>
        public Icon[]? Icons
        {
            get
            {
                if (_iconsParsed) return _icons;
                _iconsParsed = true;
                return _icons ??= _iconDirectoryLocations
                    .Select(location => new Icon(PeFile, location.Offset, location.Size, location.Resource.Parent.Parent.Parent?.ID ?? uint.MaxValue))
                    .ToArray();
            }
        }

        /// <summary>
        ///     Optional Resource member which contains GroupIcon information
        ///     and a variable number of GroupIconDirectoryEntries.
        /// </summary>
        public GroupIconDirectory[]? GroupIconDirectories
        {
            get
            {
                if (_groupIconDirectoriesParsed) return _groupIconDirectories;
                _groupIconDirectoriesParsed = true;
                return _groupIconDirectories ??= _groupIconDirectoryLocations
                    .Select(location => new GroupIconDirectory(PeFile, location.Offset))
                    .ToArray();
            }
        }

        /// <summary>
        ///     Creates a new Resource data directory instance.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of the resources.</param>
        /// <param name="vsVersionLocation">vsVersionLocation.</param>
        /// <param name="iconDirectoryLocations">iconDirectoryLocations.</param>
        /// <param name="groupIconDirectoryLocations">groupIconDirectoryLocations.</param>
        public Resources(IRawFile peFile, long offset, ResourceLocation vsVersionLocation, ResourceLocation[] iconDirectoryLocations, ResourceLocation[] groupIconDirectoryLocations)
            : base(peFile, offset)
        {
            _vsVersionLocation = vsVersionLocation;
            _iconDirectoryLocations = iconDirectoryLocations;
            _groupIconDirectoryLocations = groupIconDirectoryLocations;
        }
    }
}
