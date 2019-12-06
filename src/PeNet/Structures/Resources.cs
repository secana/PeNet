using PeNet.Utilities;

namespace PeNet.Structures
{
    public class Resources : AbstractStructure
    {
        public VsVersionInfo VsVersionInfo { get; }

        public Resources(byte[] buff, uint offset, uint vsVersionOffset) 
            : base(buff, offset)
        {
            VsVersionInfo = new VsVersionInfo(Buff, vsVersionOffset);
        }
    }

    public class VsVersionInfo : AbstractStructure
    {
        public ushort wLength
        {
            get => Buff.BytesToUInt16(Offset);
            set => Buff.SetUInt16(Offset, value);
        }

        public ushort wValueLength
        {
            get => Buff.BytesToUInt16(Offset + 0x2);
            set => Buff.SetUInt16(Offset + 0x2, value);
        }

        public ushort wType
        {
            get => Buff.BytesToUInt16(Offset + 0x4);
            set => Buff.SetUInt16(Offset + 0x4, value);
        }

        public VsVersionInfo(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }
    }
}