using System.IO;

namespace PeNet.Structures
{
    /// <summary>
    /// Parsed version of the Resource data directory.
    /// </summary>
    public class Resources : AbstractStructure
    {
        private VS_VERSIONINFO? _vsVersionInfo;
        private readonly uint _vsVersionOffset;

        /// <summary>
        /// Optional Resource member which contains version information
        /// and compile time information about the binary.
        /// </summary>
        public VS_VERSIONINFO VsVersionInfo
        {
            get
            {
                _vsVersionInfo ??= new VS_VERSIONINFO(PeFile, _vsVersionOffset);
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