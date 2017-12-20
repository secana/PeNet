using System.IO;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1BitString : Asn1Node {

        public const string NODE_NAME = "BitString";

        public byte[] Data;

        public int ExtraBitsCount { get; set; }

        public Asn1BitString() {
        }

        public Asn1BitString(byte[] data) {
            Data = data;
        }

        public static Asn1BitString ReadFrom(Stream stream) {
            var extra = stream.ReadByte();
            var data = new byte[stream.Length - stream.Position];
            stream.Read(data, 0, data.Length);

            return new Asn1BitString { Data = data, ExtraBitsCount = extra };
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.BitString;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, ToHexString(Data), new XAttribute("extra", ExtraBitsCount));
        }

        protected override byte[] GetBytesCore() {
            var mem = new MemoryStream();
            mem.WriteByte((byte)ExtraBitsCount);
            mem.Write(Data, 0, Data.Length);
            return mem.ToArray();
        }

        public new static Asn1BitString Parse(XElement xNode) {
            var res = new Asn1BitString {
                Data = ReadDataFromHexString(xNode.Value),
                ExtraBitsCount = int.Parse(xNode.Attribute("extra").Value)
            };
            return res;
        }
    }
}
