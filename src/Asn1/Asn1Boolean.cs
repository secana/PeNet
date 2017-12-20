using System.IO;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1Boolean : Asn1Node {

        public const string NODE_NAME = "Boolean";

        public bool Value { get; set; }

        public Asn1Boolean(bool value) {
            Value = value;
        }

        public Asn1Boolean() {
        }

        public static Asn1Boolean ReadFrom(Stream stream) {
            var bt = stream.ReadByte();
            return new Asn1Boolean(bt != 0);
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.Boolean;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, Value.ToString().ToLowerInvariant());
        }

        protected override byte[] GetBytesCore() {
            return new[] { (byte)(Value ? 1 : 0) };
        }

        public new static Asn1Boolean Parse(XElement xNode) {
            return new Asn1Boolean(xNode.Value.ToLowerInvariant().Trim() == "trim");
        }
    }
}
