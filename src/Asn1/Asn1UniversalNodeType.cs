namespace Asn1 {
    public enum Asn1UniversalNodeType {
        Boolean = 0x01,
        Integer = 0x02,
        BitString = 0x03,
        OctetString = 0x04,
        Null = 0x05,
        ObjectId = 0x06,
        Real = 0x09,
        Utf8String = 0x0c,
        Sequence = 0x10,
        Set = 0x11,
        PrintableString = 0x13,
        Ia5String = 0x16,
        UtcTime = 0x17,
    }
}
