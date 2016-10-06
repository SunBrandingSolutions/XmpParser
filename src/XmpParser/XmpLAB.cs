using System.Globalization;
using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// An LAB swatch.
    /// </summary>
    public class XmpLAB : XmpSwatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmpLAB" /> class.
        /// </summary>
        public XmpLAB()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpLAB" /> class.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
        public XmpLAB(XmlElement xml)
            : base(xml)
        {
            const string xmpG = "http://ns.adobe.com/xap/1.0/g/";

            Lightness = XmlUtils.TryGetDouble(xml, "L", xmpG);
            A = XmlUtils.TryGetInt(xml, "A", xmpG);
            B = XmlUtils.TryGetInt(xml, "B", xmpG);
        }

        /// <summary>
        /// Gets the lightness value.
        /// </summary>
        public double Lightness { get; }

        /// <summary>
        /// Gets the A value.
        /// </summary>
        public int A { get; }

        /// <summary>
        /// Gets the B value.
        /// </summary>
        public int B { get; }

        /// <summary>
        /// Converts this colour to a web-compatible hexadecimal string value.
        /// </summary>
        /// <returns>A hex colour string.</returns>
        public override string ToHexColor()
        {
            // TODO: implement a formula for a very basic conversion to sRGB
            return XmpSwatch.RgbToHex(0, 0, 0);
        }

        /// <summary>
        /// Gets the values as a readable string.
        /// </summary>
        /// <returns>A string in the form of '{key}={value}'.</returns>
        public override string ToValuesString() => $"L={Lightness} A={A} B={B}";
    }
}
