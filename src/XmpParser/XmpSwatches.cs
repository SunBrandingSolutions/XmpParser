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
            string xmpG = "http://ns.adobe.com/xap/1.0/g/";

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

        public abstract string ToHexColor();

        public abstract string ToValuesString();

        public override string ToString()
        {
            return Name;
        }
    }
}
