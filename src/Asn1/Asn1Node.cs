using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Asn1 {
    public abstract class Asn1Node {

        public Asn1TagClass TagClass { get; set; }

        public abstract Asn1UniversalNodeType NodeType { get; }

        public abstract Asn1TagForm TagForm { get; }

        public IList<Asn1Node> Nodes { get; } = new List<Asn1Node>();

        public Asn1Node FindSingleNode(Asn1TagClass tagClass, int tagId) {
            return Nodes.FirstOrDefault(n => n.Is(tagClass, tagId));
        }

        private static int ReadTagLength(Stream stream) {
            var len = stream.ReadByte();
            if (len > 128) {
                var lenBytes = len - 128;
                len = 0;
                for (var i = 0; i < lenBytes; i++) {
                    len = len * 256 + stream.ReadByte();
                }
            } else if (len == 128) {
                //todo: dynamic length
            }
            return len;
        }

        public static Asn1Node ReadNode(byte[] data) {
            return ReadNode(new MemoryStream(data));
        }

        public static Asn1Node ReadNode(Stream stream) {
            var identifier = stream.ReadByte();
            var type = (Asn1UniversalNodeType)(identifier & 0x1f);
            var tagClass = (Asn1TagClass)(identifier >> 6);
            var tagForm = (Asn1TagForm)((identifier >> 5) & 1);
            var length = ReadTagLength(stream);
            var data = new byte[length];
            stream.Read(data, 0, length);
            stream = new MemoryStream(data);
            if (tagClass == Asn1TagClass.Universal) {
                var tag = ReadUniversalNode(type, tagForm, stream);
                tag.TagClass = tagClass;
                return tag;
            } else {
                var tag = Asn1CustomNode.ReadFrom(type, tagForm, stream);
                tag.TagClass = tagClass;
                return tag;
            }
        }

        private static Asn1Node ReadUniversalNode(Asn1UniversalNodeType type, Asn1TagForm tagForm, Stream stream) {
            switch (type) {
                case Asn1UniversalNodeType.Boolean: return Asn1Boolean.ReadFrom(stream);
                case Asn1UniversalNodeType.Integer: return Asn1Integer.ReadFrom(stream);
                case Asn1UniversalNodeType.BitString: return Asn1BitString.ReadFrom(stream);
                case Asn1UniversalNodeType.OctetString: return Asn1OctetString.ReadFrom(stream);
                case Asn1UniversalNodeType.Null: return Asn1Null.ReadFrom(stream);
                case Asn1UniversalNodeType.ObjectId: return Asn1ObjectIdentifier.ReadFrom(stream);
                case Asn1UniversalNodeType.Utf8String: return Asn1Utf8String.ReadFrom(stream);
                case Asn1UniversalNodeType.PrintableString: return Asn1PrintableString.ReadFrom(stream);
                case Asn1UniversalNodeType.Ia5String: return Asn1Ia5String.ReadFrom(stream);
                case Asn1UniversalNodeType.UtcTime: return Asn1UtcTime.ReadFrom(stream);
                case Asn1UniversalNodeType.Sequence: return Asn1Sequence.ReadFrom(stream);
                case Asn1UniversalNodeType.Set: return Asn1Set.ReadFrom(stream);
                default:
                    throw new NotSupportedException($"unsupported universal type {type}");
            }
        }

        protected void ReadChildren(Stream stream) {
            while (stream.Position != stream.Length) {
                Nodes.Add(ReadNode(stream));
            }
        }

        public virtual XElement ToXElement() {
            var xnode = ToXElementCore();
            if (TagClass != Asn1TagClass.Universal) {
                xnode.Add(new XAttribute("class", TagClass.ToString()));
            }
            return xnode;
        }

        protected abstract XElement ToXElementCore();

        protected static string ToHexString(byte[] data) {
            const string hex = "0123456789abcdef";
            var res = data.Aggregate("", (str, bt) => str + (hex[bt >> 4].ToString() + hex[bt & 0x0f].ToString()));
            return res;
        }

        protected static byte[] ReadDataFromHexString(string src) {
            var data = new byte[src.Length / 2];
            src = src.ToLower().Trim();
            const string hex = "0123456789abcdef";
            for (var i = 0; i < data.Length; i++) {
                var h = src[i * 2];
                var l = src[i * 2 + 1];
                var bt = hex.IndexOf(h) * 16 + hex.IndexOf(l);
                data[i] = (byte)bt;
            }
            return data;
        }

        public static Asn1Node Parse(XElement xNode) {
            switch (xNode.Name.LocalName) {
                case Asn1Sequence.NODE_NAME:
                    return Asn1Sequence.Parse(xNode);
                case Asn1Set.NODE_NAME:
                    return Asn1Set.Parse(xNode);
                case Asn1Integer.NODE_NAME:
                    return Asn1Integer.Parse(xNode);
                case Asn1ObjectIdentifier.NODE_NAME: return Asn1ObjectIdentifier.Parse(xNode);
                case Asn1PrintableString.NODE_NAME:
                    return Asn1PrintableString.Parse(xNode);
                case Asn1Utf8String.NODE_NAME:
                    return Asn1Utf8String.Parse(xNode);
                case Asn1Null.NODE_NAME:
                    return Asn1Null.Parse(xNode);
                case Asn1BitString.NODE_NAME:
                    return Asn1BitString.Parse(xNode);
                case Asn1CustomNode.NODE_NAME:
                    return Asn1CustomNode.Parse(xNode);
                default:
                    throw new Exception("Invalid Node " + xNode.Name.LocalName);
            }
        }

        public byte[] GetBytes() {
            var payload = GetBytesCore();
            var type = NodeType;
            var tagClass = TagClass;
            var tagForm = TagForm;
            var mem = new MemoryStream();
            var identifier = ((int)tagClass << 6) | ((int)tagForm << 5) | (int)type;
            mem.WriteByte((byte)identifier);
            if (payload.Length < 128) {
                mem.WriteByte((byte)payload.Length);
            } else {
                if (payload.Length < 0x100) {
                    mem.WriteByte(0x81);
                    mem.WriteByte((byte)payload.Length);
                } else if (payload.Length < 0x10000) {
                    mem.WriteByte(0x82);
                    mem.WriteByte((byte)(payload.Length >> 8));
                    mem.WriteByte((byte)(payload.Length & 0xff));
                } else {
                    throw new NotImplementedException();
                }
            }
            mem.Write(payload, 0, payload.Length);
            return mem.ToArray();
        }

        protected abstract byte[] GetBytesCore();

        public bool Is(Asn1TagClass @class, int tagId) {
            return TagClass == @class && (int)NodeType == tagId;
        }

        public bool Is(Asn1UniversalNodeType nodeType) {
            return TagClass == Asn1TagClass.Universal && NodeType == nodeType;
        }
    }
}