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

        /// <summary>
        /// Create a printable string representation of the object.
        /// </summary>
        /// <returns>String containing all property-value pairs.</returns>
        public override string ToString()
        {
            var obj = this;
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sb = new StringBuilder();
            sb.Append($"{obj.GetType().Name}\n");

            foreach (var p in properties)
            {
                if (p.PropertyType.IsArray)
                {
                    if(p.GetValue(obj, null) == null)
                        continue;

                    foreach(var entry in (IEnumerable) p.GetValue(obj, null))
                    {
                        if(entry.GetType().IsSubclassOf(typeof(AbstractStructure)) == false)
                            continue;

                        sb.Append(entry.ToString());
                    }
                }
                else
                {
                    sb.AppendFormat("{0}: {1}\n", p.Name, p.GetValue(obj, null));
                }
            }

            return sb.ToString();
        }
    }
}