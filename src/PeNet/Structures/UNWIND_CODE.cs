using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     The UNWIND_CODE is a struct in
    ///     the UNWIND_INFO used to describe
    ///     exception handling in x64 applications
    ///     and to walk the stack.
    /// </summary>
    public class UNWIND_CODE : AbstractStructure
    {
        /// <summary>
        ///     Create a new UNWIND_INFO object.
        /// </summary>
        /// <param name="buff">A PE file as a byte array.</param>
        /// <param name="offset">Raw offset of the UNWIND_INFO.</param>
        public UNWIND_CODE(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        /// <summary>
        ///     Code offset.
        /// </summary>
        public byte CodeOffset
        {
            get { return Buff[Offset]; }
            set { Buff[Offset] = value; }
        }

        /// <summary>
        ///     Unwind operation.
        /// </summary>
        public byte UnwindOp => (byte) (Buff[Offset + 0x1] >> 4);

        /// <summary>
        ///     Operation information.
        /// </summary>
        public byte Opinfo => (byte) (Buff[Offset + 0x1] & 0xF);

        /// <summary>
        ///     Frame offset.
        /// </summary>
        public ushort FrameOffset
        {
            get { return Buff.BytesToUInt16(Offset + 0x2); }
            set { Buff.SetUInt16(Offset + 0x2, value); }
        }

        /// <summary>
        ///     Creates a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>UNWIND_CODE properties as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("UNWIND_CODE\n");
            sb.Append(this.PropertiesToString("{0,-20}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}