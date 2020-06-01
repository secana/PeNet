namespace PeNet.Header.Pe
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
        /// <param name="iatOffset">Offset into the Import Address Table.</param>
        public ImportFunction(string? name, string dll, ushort hint, uint iatOffset)
        {
            Name = name;
            DLL = dll;
            Hint = hint;
            IATOffset = iatOffset;
        }

        /// <summary>
        ///     Function name.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        ///     DLL where the function comes from.
        /// </summary>
        public string DLL { get; }

        /// <summary>
        ///     Function hint.
        /// </summary>
        public ushort Hint { get; }

        /// <summary>
        ///     Offset into the Import Address Table.
        /// </summary>
        public uint IATOffset { get; }
    }
}