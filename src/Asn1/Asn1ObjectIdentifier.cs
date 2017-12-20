using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1ObjectIdentifier : Asn1Node {

        public const string NODE_NAME = "ObjectId";

        private ulong[] Sids { get; }

        public Asn1ObjectIdentifier(ulong[] sids) {
            Sids = sids;
            if (Sids[0] > 2) throw new FormatException("first sid must be 0, 1 or 2");
        }

        public Asn1ObjectIdentifier(string value) {
            Sids = value.Split('.').Select(ulong.Parse).ToArray();
            if (Sids[0] > 2) throw new FormatException("first sid must be 0, 1 or 2");
        }

        public static bool operator ==(Asn1ObjectIdentifier first, Asn1ObjectIdentifier second) {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null)) return true;
            if (ReferenceEquals(first, null) ^ ReferenceEquals(second, null)) return false;
            if (first.Sids.Length != second.Sids.Length) return false;
            for (var i = 0; i < first.Sids.Length; i++) {
                var firstSid = first.Sids[i];
                var secondSid = second.Sids[i];
                if (firstSid != secondSid) return false;
            }
            return true;
        }

        public static bool operator !=(Asn1ObjectIdentifier first, Asn1ObjectIdentifier second) {
            return !(first == second);
        }

        protected bool Equals(Asn1ObjectIdentifier other) {
            return ToString().Equals(other.ToString());
        }

        public override bool Equals(object obj) {
            return ToString().Equals(obj.ToString());
        }

        public override int GetHashCode() {
            return ToString().GetHashCode();
        }

        public static Asn1ObjectIdentifier ReadFrom(Stream stream) {
            var res = new List<ulong>();
            while (stream.Position < stream.Length) {
                var val = ReadUInt64(stream);
                if (res.Count == 0) {
                    if (val < 40) {
                        res.Add(0);
                    } else if (val < 80) {
                        res.Add(1);
                        val -= 40;
                    } else {
                        res.Add(2);
                        val -= 80;
                    }
                }
                res.Add(val);
            }
            return new Asn1ObjectIdentifier(res.ToArray());
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.ObjectId;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, string.Join(".", Sids.Select(s => s.ToString())));
        }

        protected override byte[] GetBytesCore() {
            using (var mem = new MemoryStream()) {
                var first = Sids[0] * 40ul + Sids[1];
                Write(mem, first);
                for (var i = 2; i < Sids.Length; i++) {
                    var sid = Sids[i];
                    Write(mem, sid);
                }
                return mem.ToArray();
            }
        }

        private static ulong ReadUInt64(Stream stream) {
            ulong val = 0;
            while (true) {
                var b = stream.ReadByte();
                val = (val << 7) + (ulong)(b & 0x7f);
                if (b < 0x80) {
                    return val;
                }
            }
        }

        private static void Write(Stream mem, ulong val) {
            var zero = true;
            for (var i = 9; i >= 0; i--) {
                var subval = (val >> (i * 7)) & 0x7ful;
                if (!zero || subval != 0) {
                    zero = false;
                    if (i != 0) subval |= 0x80;
                    mem.WriteByte((byte)subval);
                }
            }
        }

        public new static Asn1ObjectIdentifier Parse(XElement xNode) {
            var val = xNode.Value.Trim();
            return new Asn1ObjectIdentifier(val);
        }


        public string Value {
            get {
                return string.Join(".", Sids.Select(s => s.ToString()));
            }
        }

        public string FriendlyName {
            get {
                var res = Value;
                switch (res) {
                    case "1.2.840.113549.1.1.1": return "rsaEncryption";
                    case "1.2.840.113549.1.1.5": return "sha1WithRSAEncryption";
                    case "1.2.840.113549.1.1.11": return "sha256WithRSAEncryption";
                    case "1.3.6.1.5.5.7.1.1": return "authorityInfoAccess";
                    case "2.5.4.3": return "commonName";
                    case "2.5.4.6": return "countryName";
                    case "2.5.4.10": return "organizationName";
                    case "2.5.4.11": return "organizationalUnitName";
                    case "2.5.29.15": return "keyUsage";
                    case "2.5.29.17": return "subjectAltName";
                    case "2.5.29.19": return "basicConstraint";
                    case "2.5.29.31": return "cRLDistributionPoints";
                    case "2.5.29.32": return "certificatePolicies";
                    case "2.5.29.35": return "authorityKeyIdentifier";
                    case "2.5.29.37": return "extKeyUsage";
                }
                return res;
            }
        }

        public override string ToString() {
            return FriendlyName;
        }

        /// <summary>
        /// 2.5.4.3
        /// </summary>
        public static Asn1ObjectIdentifier CommonName { get; } = new Asn1ObjectIdentifier("2.5.4.3");
        /// <summary>
        /// 2.5.4.6
        /// </summary>
        public static Asn1ObjectIdentifier CountryName { get; } = new Asn1ObjectIdentifier("2.5.4.6");
        /// <summary>
        /// 2.5.4.7
        /// </summary>
        public static Asn1ObjectIdentifier LocalityName { get; } = new Asn1ObjectIdentifier("2.5.4.7");
        /// <summary>
        /// 2.5.4.9
        /// </summary>
        public static Asn1ObjectIdentifier StreetAddress { get; } = new Asn1ObjectIdentifier("2.5.4.9");
        /// <summary>
        /// 2.5.4.10
        /// </summary>
        public static Asn1ObjectIdentifier OrganizationName { get; } = new Asn1ObjectIdentifier("2.5.4.10");
        /// <summary>
        /// 2.5.4.11
        /// </summary>
        public static Asn1ObjectIdentifier OrganizationalUnitName { get; } = new Asn1ObjectIdentifier("2.5.4.11");

        #region Extensions Ids

        /// <summary>
        /// 2.5.29.14
        /// </summary>
        public static Asn1ObjectIdentifier SubjectKeyIdentifier { get; } = new Asn1ObjectIdentifier("2.5.29.14");
        /// <summary>
        /// 2.5.29.15
        /// </summary>
        public static Asn1ObjectIdentifier KeyUsage { get; } = new Asn1ObjectIdentifier("2.5.29.15");
        /// <summary>
        /// 2.5.29.17
        /// </summary>
        public static Asn1ObjectIdentifier SubjectAltName { get; } = new Asn1ObjectIdentifier("2.5.29.17");
        /// <summary>
        /// 2.5.29.18
        /// </summary>
        public static Asn1ObjectIdentifier IssuerAltName { get; } = new Asn1ObjectIdentifier("2.5.29.18");
        /// <summary>
        /// 2.5.29.19
        /// </summary>
        public static Asn1ObjectIdentifier BasicConstraints { get; } = new Asn1ObjectIdentifier("2.5.29.19");
        /// <summary>
        /// 2.5.29.31
        /// </summary>
        public static Asn1ObjectIdentifier CrlDistributionPoints { get; } = new Asn1ObjectIdentifier("2.5.29.31");
        /// <summary>
        /// 2.5.29.32
        /// </summary>
        public static Asn1ObjectIdentifier CertificatePolicies { get; } = new Asn1ObjectIdentifier("2.5.29.32");
        /// <summary>
        /// 2.5.29.35
        /// </summary>
        public static Asn1ObjectIdentifier AuthorityKeyIdentifier { get; } = new Asn1ObjectIdentifier("2.5.29.35");
        /// <summary>
        /// 2.5.29.37
        /// </summary>
        public static Asn1ObjectIdentifier ExtKeyUsage { get; } = new Asn1ObjectIdentifier("2.5.29.37");

        #endregion

        /// <summary>
        /// 1.2.840.113549.1.1.1
        /// </summary>
        public static Asn1ObjectIdentifier RsaEncryption { get; } = new Asn1ObjectIdentifier("1.2.840.113549.1.1.1");
        /// <summary>
        /// 1.2.840.113549.1.1.5
        /// </summary>
        public static Asn1ObjectIdentifier Sha1WithRsaEncryption { get; } = new Asn1ObjectIdentifier("1.2.840.113549.1.1.5");
        /// <summary>
        /// 1.2.840.113549.1.1.11
        /// </summary>
        public static Asn1ObjectIdentifier Sha256WithRsaEncryption { get; } = new Asn1ObjectIdentifier("1.2.840.113549.1.1.11");
        /// <summary>
        /// 1.2.840.113549.1.9.20
        /// </summary>
        //public static Asn1ObjectIdentifier FriendlyName { get; } = new Asn1ObjectIdentifier("1.2.840.113549.1.9.20");

        /// <summary>
        /// 1.3.6.1.5.5.7.1.1
        /// </summary>
        public static Asn1ObjectIdentifier AuthorityInfoAccess { get; } = new Asn1ObjectIdentifier("1.3.6.1.5.5.7.1.1");

        /// <summary>
        /// 1.3.14.3.2.26
        /// </summary>
        public static Asn1ObjectIdentifier Sha1 { get; } = new Asn1ObjectIdentifier("1.3.14.3.2.26");
    }
}
