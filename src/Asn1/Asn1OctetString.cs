using System.IO;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1OctetString : Asn1Node {

        public const string NODE_NAME = "OctetString";

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.OctetString;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        public byte[] Data { get; set; }

        public Asn1OctetString() {
        }

        public Asn1OctetString(byte[] data) {
            Data = data;
        }

        public static Asn1OctetString ReadFrom(Stream stream) {
            var data = new byte[stream.Length - stream.Position];
            stream.Read(data, 0, data.Length);

            return new Asn1OctetString { Data = data };
        }

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, ToHexString(Data));
        }

        protected override byte[] GetBytesCore() {
            var mem = new MemoryStream();
            mem.Write(Data, 0, Data.Length);
            return mem.ToArray();
        }

        public new static Asn1OctetString Parse(XElement xNode) {
            var res = new Asn1OctetString {
                Data = ReadDataFromHexString(xNode.Value),
            };
            return res;
        }

    }
}
