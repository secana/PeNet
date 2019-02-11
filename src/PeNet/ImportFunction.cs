using System.Text;
using ExtensionMethods = PeNet.Utilities.ExtensionMethods;

namespace PeNet
{
    /// <summary>
    ///     Represents an imported function.
    /// </summary>
    public class ImportFunction
    {
        /// <summary>
        ///     Create a new ImportFunction object.
        /// </summary>
        /// <param name="name">Function name.</param>
        /// <param name="dll">DLL where the function comes from.</param>
        /// <param name="hint">Function hint.</param>
        public ImportFunction(string name, string dll, ushort hint)
        {
            Name = name;
            DLL = dll;
            Hint = hint;
        }

        /// <summary>
        ///     Function name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     DLL where the function comes from.
        /// </summary>
        public string DLL { get; }

        /// <summary>
        ///     Function hint.
        /// </summary>
        public ushort Hint { get; }
    }
}