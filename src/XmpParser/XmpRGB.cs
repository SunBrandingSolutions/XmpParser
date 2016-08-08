using System.Globalization;
using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// An RGB swatch.
    /// </summary>
    public class XmpRGB : XmpSwatch
    {
        public XmpRGB()
            : base()
        {
        }

        public XmpRGB(XmlElement xml)
            : base(xml)
        {
            const string xmpG = "http://ns.adobe.com/xap/1.0/g/";

            Red = XmlUtils.TryGetByte(xml, "red", xmpG);
            Green = XmlUtils.TryGetByte(xml, "green", xmpG);
            Blue = XmlUtils.TryGetByte(xml, "blue", xmpG);
        }

        public byte Red { get; }

        public byte Green { get; }

        public byte Blue { get; }

        public override string ToHexColor()
        {
            return XmpSwatch.RgbToHex(Red, Green, Blue);
        }

        public override string ToValuesString() => $"R={Red} G={Green} B={Blue}";
    }
}
