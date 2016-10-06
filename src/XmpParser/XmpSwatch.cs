using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// Base class for XMP Color swatches.
    /// </summary>
    public abstract class XmpSwatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmpSwatch" /> class.
        /// </summary>
        protected XmpSwatch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpSwatch" /> class.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
        protected XmpSwatch(XmlElement xml)
        {
            const string xmpG = "http://ns.adobe.com/xap/1.0/g/";

            Type = XmlUtils.TryGetEnum<SwatchType>(xml, "type", xmpG);
            Name = XmlUtils.TryGetValue(xml, "swatchName", xmpG);
            Mode = XmlUtils.TryGetValue(xml, "mode", xmpG);
        }

        /// <summary>
        /// Gets the swatch type.
        /// </summary>
        public SwatchType Type { get; }

        /// <summary>
        /// Gets the swatch name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the swatch mode.
        /// </summary>
        public string Mode { get; }

        /// <summary>
        /// Factory method to create a strongly-typed XMP swatch from an XML element.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
        /// <returns>An <see cref="XmpSwatch" /> object.</returns>
        public static XmpSwatch Create(XmlElement xml)
        {
            string xmpG = "http://ns.adobe.com/xap/1.0/g/";
            var mode = XmlUtils.TryGetValue(xml, "mode", xmpG);

            switch (mode)
            {
                case "RGB":
                    return new XmpRGB(xml);
                case "CMYK":
                    return new XmpCMYK(xml);
                case "LAB":
                    return new XmpLAB(xml);
                case "GRAY":
                    return new XmpGray(xml);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts an RGB color value to its hexadecimal equivalent.
        /// </summary>
        /// <param name="r">Red color value</param>
        /// <param name="g">Green color value</param>
        /// <param name="b">Blue color value</param>
        /// <returns>An HTML-compatible color hex value.</returns>
        public static string RgbToHex(byte r, byte g, byte b) => $"{r:X2}{g:X2}{b:X2}";

        /// <summary>
        /// Converts this colour to a web-compatible hexadecimal string value.
        /// </summary>
        /// <returns>A hex colour string.</returns>
        public abstract string ToHexColor();

        /// <summary>
        /// Gets the values as a readable string.
        /// </summary>
        /// <returns>A string in the form of '{key}={value}'.</returns>
        public abstract string ToValuesString();
    }
}
