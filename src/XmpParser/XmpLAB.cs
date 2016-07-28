using System.Globalization;
using System.Xml;

namespace XmpParser
{
    public class XmpLAB : XmpSwatch
    {
        public XmpLAB()
            : base()
        {
        }

        public XmpLAB(XmlElement xml)
            : base(xml)
        {
            string xmpG = "http://ns.adobe.com/xap/1.0/g/";

            Lightness = XmlUtils.TryGetDouble(xml, "L", xmpG);
            A = XmlUtils.TryGetInt(xml, "A", xmpG);
            B = XmlUtils.TryGetInt(xml, "B", xmpG);
        }

        public double Lightness { get; }

        public int A { get; }

        public int B { get; }

        public override string ToHexColor()
        {
            // TODO: implement a formula for a very basic conversion to sRGB
            return XmpUtils.RgbToHex(0, 0, 0);
        }

        public override string ToValuesString()
        {
            return string.Format(CultureInfo.InvariantCulture, "L={0} A={1} B={2}", Lightness, A, B);
        }
    }
}
