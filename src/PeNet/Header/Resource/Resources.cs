using PeNet.FileParser;

namespace PeNet.Header.Resource
{
    /// <summary>
    /// Parsed version of the Resource data directory.
    /// </summary>
    public class Resources : AbstractStructure
    {
        private VsVersionInfo? _vsVersionInfo;
        private readonly uint _vsVersionOffset;

        /// <summary>
        /// Optional Resource member which contains version information
        /// and compile time information about the binary.
        /// </summary>
        public VsVersionInfo VsVersionInfo
        {
            get
            {
                _vsVersionInfo ??= new VsVersionInfo(PeFile, _vsVersionOffset);
                return _vsVersionInfo;
            }
        }

        /// <summary>
        /// Creates a new Resource data directory instance.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of the resources.</param>
        /// <param name="vsVersionOffset">vsVersionOffset.</param>
        public Resources(IRawFile peFile, long offset, uint vsVersionOffset) 
            : base(peFile, offset)
        {
            _vsVersionOffset = vsVersionOffset;
        }
    }
}