using System;
using System.IO;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1CustomNode : Asn1Node {

        public const string NODE_NAME = "Custom";

        public byte[] Data { get; set; }

        public Asn1CustomNode(int type, Asn1TagForm tagForm) {
            Type = (Asn1UniversalNodeType)type;
            TagForm = tagForm;
        }

        public Asn1CustomNode(int type, Asn1TagForm tagForm, Asn1TagClass tagClass) {
            Type = (Asn1UniversalNodeType)type;
            TagForm = tagForm;
            TagClass = tagClass;
        }

        public Asn1UniversalNodeType Type { get; }

        public static Asn1CustomNode ReadFrom(Asn1UniversalNodeType type, Asn1TagForm tagForm, Stream stream) {
            if (tagForm == Asn1TagForm.Primitive) {
                var data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);

                return new Asn1CustomNode((int)type, tagForm) { Data = data };
            } else {
                var res = new Asn1CustomNode((int)type, tagForm);
                res.ReadChildren(stream);
                return res;
            }
        }

        public override Asn1UniversalNodeType NodeType => Type;

        public override Asn1TagForm TagForm { get; }


        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME,
                new XAttribute("type", Type),
                new XAttribute("form", TagForm),
                ToHexString(Data));
        }

        protected override byte[] GetBytesCore() {
            if (TagForm == Asn1TagForm.Primitive) {
                return Data;
            }
            var mem = new MemoryStream();
            foreach (var node in Nodes) {
                var data = node.GetBytes();
                mem.Write(data, 0, data.Length);
            }
            return mem.ToArray();
        }

        public new static Asn1CustomNode Parse(XElement xNode) {
            var type = int.Parse(xNode.Attribute("type").Value);
            var form = (Asn1TagForm)Enum.Parse(typeof(Asn1TagForm), xNode.Attribute("form").Value);
            var tagClass = (Asn1TagClass)Enum.Parse(typeof(Asn1TagClass), xNode.Attribute("class").Value);
            var res = new Asn1CustomNode(type, form) { TagClass = tagClass };
            res.Data = ReadDataFromHexString(xNode.Value);
            return res;
        }
    }
}
