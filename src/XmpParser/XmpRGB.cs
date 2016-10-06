using System.Globalization;
using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// An RGB swatch.
    /// </summary>
    public class XmpRGB : XmpSwatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmpRGB" /> class.
        /// </summary>
        public XmpRGB()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpRGB" /> class.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
        public XmpRGB(XmlElement xml)
            : base(xml)
        {
            const string xmpG = "http://ns.adobe.com/xap/1.0/g/";

            Red = XmlUtils.TryGetByte(xml, "red", xmpG);
            Green = XmlUtils.TryGetByte(xml, "green", xmpG);
            Blue = XmlUtils.TryGetByte(xml, "blue", xmpG);
        }

        /// <summary>
        /// Gets the red value.
        /// </summary>
        public byte Red { get; }

        /// <summary>
        /// Gets the green value.
        /// </summary>
        public byte Green { get; }

        /// <summary>
        /// Gets the blue value.
        /// </summary>
        public byte Blue { get; }

        /// <summary>
        /// Converts this colour to a web-compatible hexadecimal string value.
        /// </summary>
        /// <returns>A hex colour string.</returns>
        public override string ToHexColor()
        {
            return XmpSwatch.RgbToHex(Red, Green, Blue);
        }

        /// <summary>
        /// Gets the values as a readable string.
        /// </summary>
        /// <returns>A string in the form of '{key}={value}'.</returns>
        public override string ToValuesString() => $"R={Red} G={Green} B={Blue}";
    }
}
