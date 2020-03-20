using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The copyright ASCII (not 0-terminated) string of the PE file
    ///     if any is given.
    /// </summary>
    public class Copyright : AbstractStructure
    {
        /// <summary>
        ///     Create a new copyright object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset to the copyright string in the binary.</param>
        public Copyright(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     The copyright string.
        /// </summary>
        public string CopyrightString => PeFile.ReadAsciiString(Offset);
    }
}