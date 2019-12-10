namespace PeNet.Structures
{
    /// <summary>
    /// Parsed version of the Resource data directory.
    /// </summary>
    public class Resources : AbstractStructure
    {
        /// <summary>
        /// Optional Resource member which contains version information
        /// and compile time information about the binary.
        /// </summary>
        public VS_VERSIONINFO VsVersionInfo { get; }

        public Resources(byte[] buff, uint offset, uint vsVersionOffset) 
            : base(buff, offset)
        {
            VsVersionInfo = new VS_VERSIONINFO(Buff, vsVersionOffset);
        }
    }
}