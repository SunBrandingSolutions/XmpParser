using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// Describes a font.
    /// </summary>ß
    public class XmpFontInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmpFontInfo" /> class.
        /// </summary>
        public XmpFontInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpFontInfo" /> class.
        /// </summary>
        /// <param name="name">The font name</param>
        /// <param name="family">The font family</param>
        /// <param name="face">The font face</param>
        /// <param name="type">The font type</param>
        public XmpFontInfo(string name, string family = "", string face = "", string type = "")
        {
            Name = name;
            Family = family;
            Face = face;
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpFontInfo" /> class.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
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

        /// <summary>
        /// Gets the font family.
        /// </summary>
        public string Family { get; }

        /// <summary>
        /// Gets the font face.
        /// </summary>
        public string Face { get; }

        /// <summary>
        /// Gets the font type.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the font name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the font version.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the font file name.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Returns a string representation of this font.
        /// </summary>
        /// <returns>The font name.</returns>
        public override string ToString()
        {
            return Name ?? "Unknown font";
        }
    }
}
