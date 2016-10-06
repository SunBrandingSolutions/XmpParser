using System.Globalization;
using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// A gray swatch.
    /// </summary>
    public class XmpGray : XmpSwatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmpGray" /> class.
        /// </summary>
        public XmpGray()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpGray" /> class.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
        public XmpGray(XmlElement xml)
            : base(xml)
        {
            const string xmpG = "http://ns.adobe.com/xap/1.0/g/";

            Gray = XmlUtils.TryGetByte(xml, "gray", xmpG);
        }

        /// <summary>
        /// Gets the gray value.
        /// </summary>
        public byte Gray { get; }

        /// <summary>
        /// Converts this colour to a web-compatible hexadecimal string value.
        /// </summary>
        /// <returns>A hex colour string.</returns>
        public override string ToHexColor()
        {
            return XmpSwatch.RgbToHex(Gray, Gray, Gray);
        }

        /// <summary>
        /// Gets the values as a readable string.
        /// </summary>
        /// <returns>A string in the form of '{key}={value}'.</returns>
        public override string ToValuesString() => "K={Gray}";
    }
}
