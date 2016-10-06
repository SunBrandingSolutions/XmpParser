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
        /// <summary>
        /// Initializes a new instance of the <see cref="XmpCMYK" /> class.
        /// </summary>
        public XmpCMYK()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpCMYK" /> class.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
        public XmpCMYK(XmlElement xml)
            : base(xml)
        {
            const string xmpG = "http://ns.adobe.com/xap/1.0/g/";

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

        /// <summary>
        /// Gets the Cyan value.
        /// </summary>
        public double Cyan { get; }

        /// <summary>
        /// Gets the Magenta value.
        /// </summary>
        public double Magenta { get; }

        /// <summary>
        /// Gets the Yellow value.
        /// </summary>
        public double Yellow { get; }

        /// <summary>
        /// Gets the Black value.
        /// </summary>
        public double Black { get; }

        /// <summary>
        /// Gets the Tint value.
        /// </summary>
        public double Tint { get; }

        /// <summary>
        /// Converts this colour to a web-compatible hexadecimal string value.
        /// </summary>
        /// <returns>A hex colour string.</returns>
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

            return XmpSwatch.RgbToHex(r, g, b);
        }

        /// <summary>
        /// Gets the values as a readable string.
        /// </summary>
        /// <returns>A string in the form of '{key}={value}'.</returns>
        public override string ToValuesString() => "C={Cyan} M={Magenta} Y={Yellow} K={Black}";

        private static byte TryConvertToByte(double value)
        {
            if (value >= 0 && value <= 255)
            {
                return (byte)Math.Ceiling(value);
            }
            else
            {
                return 0;
            }
        }
    }
}