using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1Ia5String : Asn1Node {

        public const string NODE_NAME = "IA5";

        public string Value { get; set; }

        public Asn1Ia5String() {
        }

        public Asn1Ia5String(string value) {
            Value = value;
        }

        public static Asn1Ia5String ReadFrom(Stream stream) {
            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            return new Asn1Ia5String { Value = Encoding.ASCII.GetString(data) };
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.Ia5String;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, Value);
        }

        protected override byte[] GetBytesCore() {
            return Encoding.ASCII.GetBytes(Value);
        }

        public new static Asn1Ia5String Parse(XElement xNode) {
            var res = new Asn1Ia5String {Value = xNode.Value.Trim()};
            //todo should it be trimmed?
            return res;
        }
    }
}
