namespace PeNet.Structures
{
    /// <summary>
    /// Parsed version of the Resource data directory.
    /// </summary>
    public class Resources : AbstractStructure
    {
        private VS_VERSIONINFO _vsVersionInfo;
        private readonly uint _vsVersionOffset;

        /// <summary>
        /// Optional Resource member which contains version information
        /// and compile time information about the binary.
        /// </summary>
        public VS_VERSIONINFO VsVersionInfo
        {
            get
            {
                _vsVersionInfo ??= new VS_VERSIONINFO(Buff, _vsVersionOffset);
                return _vsVersionInfo;
            }
        }

        public Resources(byte[] buff, uint offset, uint vsVersionOffset) 
            : base(buff, offset)
        {
            _vsVersionOffset = vsVersionOffset;
        }
    }
}