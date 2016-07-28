using System;
using System.Globalization;
using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// A CMYK swatch.
    /// </summary>
    public class XmpCMYK : XmpSwatch
    {
        public XmpCMYK()
            : base()
        {
        }

        public XmpCMYK(XmlElement xml)
            : base(xml)
        {
            string xmpG = "http://ns.adobe.com/xap/1.0/g/";

            Cyan = XmlUtils.TryGetDouble(xml, "cyan", xmpG);
            Magenta = XmlUtils.TryGetDouble(xml, "magenta", xmpG);
            Yellow = XmlUtils.TryGetDouble(xml, "yellow", xmpG);
            Black = XmlUtils.TryGetDouble(xml, "black", xmpG);
            Tint = XmlUtils.TryGetDouble(xml, "tint", xmpG);

            if (Cyan > 1 || Magenta > 1 || Yellow > 1 || Black > 1)
            {
                Cyan = Cyan / 100;
                Magenta = Magenta / 100;
                Yellow = Yellow / 100;
                Black = Black / 100;
            }
        }

        public double Cyan { get; set; }

        public double Magenta { get; set; }

        public double Yellow { get; set; }

        public double Black { get; set; }

        public double Tint { get; set; }

        public override string ToHexColor()
        {
            /* please note that this is only an approximate conversion
             * and will probably not match exactly what appears in
             * Photoshop/Illustrator; it's intended to be used for quick
             * display and preview purposes only.
             *
             * algorithm kindly provided by http://www.rapidtables.com/convert/color/cmyk-to-rgb.htm
             * */

            var r = TryConvertToByte(255 * (1 - Cyan) * (1 - Black));
            var g = TryConvertToByte(255 * (1 - Magenta) * (1 - Black));
            var b = TryConvertToByte(255 * (1 - Yellow) * (1 - Black));

            return XmpUtils.RgbToHex(r, g, b);
        }

        public override string ToValuesString()
        {
            return string.Format(CultureInfo.InvariantCulture, "C={0} M={1} Y={2} K={3}", Cyan, Magenta, Yellow, Black);
        }

        private static byte TryConvertToByte(double value)
        {
            if (value >= 0 && value <= 255)
                return (byte)Math.Ceiling(value);
            else
                return 0;
        }
    }
}