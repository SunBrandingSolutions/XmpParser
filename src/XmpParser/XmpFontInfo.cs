namespace XmpParser
{
    using System.Xml;

    public class XmpFontInfo
    {
        public XmpFontInfo()
        {
        }

        public XmpFontInfo(string name, string family = "", string face = "", string type = "")
        {
            Name = name;
            Family = family;
            Face = face;
            Type = type;
        }

        public XmpFontInfo(XmlElement xml)
        {
            const string stFnt = "http://ns.adobe.com/xap/1.0/sType/Font#";

            Name = XmlUtils.TryGetValue(xml, "fontName", stFnt);
            Family = XmlUtils.TryGetValue(xml, "fontFamily", stFnt);
            Face = XmlUtils.TryGetValue(xml, "fontFace", stFnt);
            Type = XmlUtils.TryGetValue(xml, "fontType", stFnt);
            Version = XmlUtils.TryGetValue(xml, "versionString", stFnt);
            FileName = XmlUtils.TryGetValue(xml, "fontFileName", stFnt);
        }

        public string Family { get; }

        public string Face { get; }

        public string Type { get; }

        public string Name { get; }

        public string Version { get; }

        public string FileName { get; }

        public override string ToString()
        {
            return Name ?? "Unknown font";
        }
    }
}
