using System.Globalization;
using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// A gray swatch.
    /// </summary>
    public class XmpGray : XmpSwatch
    {
        public XmpGray()
            : base()
        {
        }

        public XmpGray(XmlElement xml)
            : base(xml)
        {
            string xmpG = "http://ns.adobe.com/xap/1.0/g/";

            Gray = XmlUtils.TryGetByte(xml, "gray", xmpG);
        }

        public byte Gray { get; }

        public override string ToHexColor()
        {
            return XmpUtils.RgbToHex(Gray, Gray, Gray);
        }

        public override string ToValuesString()
        {
            return string.Format(CultureInfo.InvariantCulture, "K={0}", Gray);
        }
    }
}
