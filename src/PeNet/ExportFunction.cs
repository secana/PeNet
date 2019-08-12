using System.Text;
using ExtensionMethods = PeNet.Utilities.ExtensionMethods;

namespace PeNet
{
    /// <summary>
    ///     Represents an exported function.
    /// </summary>
    public class ExportFunction
    {
        /// <summary>
        ///     Create a new ExportFunction object.
        /// </summary>
        /// <param name="name">Name of the function.</param>
        /// <param name="address">Address of function.</param>
        /// <param name="ordinal">Ordinal of the function.</param>
        public ExportFunction(string name, uint address, ushort ordinal)
        {
            Name = name;
            Address = address;
            Ordinal = ordinal;
        }

        /// <summary>
        ///     Create a new ExportFunction object.
        /// </summary>
        /// <param name="name">Name of the function.</param>
        /// <param name="address">Address of function.</param>
        /// <param name="ordinal">Ordinal of the function.</param>
        /// <param name="forwardName">Name of the DLL and function this export forwards to.</param>
        public ExportFunction(string name, uint address, ushort ordinal, string forwardName)
        {
            Name = name;
            Address = address;
            Ordinal = ordinal;
            ForwardName = forwardName;
        }

        /// <summary>
        ///     Function name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Function RVA.
        /// </summary>
        public uint Address { get; }

        /// <summary>
        ///     Function Ordinal.
        /// </summary>
        public ushort Ordinal { get; }

        /// <summary>
        ///     Function name if the function is
        ///     forwarded to another DLL.
        ///     Format "DLLName.ExportName".
        /// </summary>
        public string ForwardName { get; }


        /// <summary>
        ///     True if the export has a name and is not forwarded.
        /// </summary>
        public bool HasName => !string.IsNullOrEmpty(Name);

        /// <summary>
        ///     True if the export has an ordinal.
        /// </summary>
        public bool HasOrdinal => Ordinal != 0;

        /// <summary>
        ///     True if the export is forwared and has 
        ///     a ForwardName.
        /// </summary>
        public bool HasForwad => !string.IsNullOrEmpty(ForwardName);
    }
}