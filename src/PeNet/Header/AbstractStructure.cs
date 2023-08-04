using System.Collections;
using System.Reflection;
using System.Text;
using PeNet.FileParser;

namespace PeNet.Header
{
    /// <summary>
    ///     Abstract class for a Windows structure.
    /// </summary>
    public abstract class AbstractStructure
    {
        /// <summary>
        ///     A PE file.
        /// </summary>
        internal readonly IRawFile PeFile;

        /// <summary>
        ///     The offset to the structure in the buffer.
        /// </summary>
        internal readonly long Offset;


        /// <summary>
        ///     Creates a new AbstractStructure which holds fields
        ///     that all structures have in common.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">The offset to the structure in the buffer.</param>
        protected AbstractStructure(IRawFile peFile, long offset)
        {
            PeFile = peFile;
            Offset = offset;
        }
    }
}
