using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// Base class for XMP Color swatches.
    /// </summary>
    public abstract class XmpSwatch
    {
        protected XmpSwatch()
        {
        }

        protected XmpSwatch(XmlElement xml)
        {
            const string xmpG = "http://ns.adobe.com/xap/1.0/g/";

            Type = XmlUtils.TryGetEnum<SwatchType>(xml, "type", xmpG);
            Name = XmlUtils.TryGetValue(xml, "swatchName", xmpG);
            Mode = XmlUtils.TryGetValue(xml, "mode", xmpG);
        }

        public SwatchType Type { get; }

        public string Name { get; }

        public string Mode { get; }

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

        public abstract string ToHexColor();

        public abstract string ToValuesString();

        public override string ToString()
        {
            return Name;
        }
    }
}
