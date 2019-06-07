using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PeNet.Structures
{
    /// <summary>
    ///     Abstract class for a Windows structure.
    /// </summary>
    public abstract class AbstractStructure
    {
        /// <summary>
        ///     A PE file as a binary buffer.
        /// </summary>
        internal readonly byte[] Buff;

        /// <summary>
        ///     The offset to the structure in the buffer.
        /// </summary>
        internal readonly uint Offset;


        /// <summary>
        ///     Creates a new AbstractStructure which holds fields
        ///     that all structures have in common.
        /// </summary>
        /// <param name="buff">A PE file as a binary buffer.</param>
        /// <param name="offset">The offset to the structure in the buffer.</param>
        protected AbstractStructure(byte[] buff, uint offset)
        {
            Buff = buff;
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

        /// <summary>
        /// Create a JSON string representation of the object.
        /// </summary>
        /// <returns>JSON string containing all property-value pairs.</returns>
        public string ToJson(bool formatted = false)
        {
            return JsonConvert.SerializeObject(this, formatted ? Formatting.Indented : Formatting.None);
        }
    }
}