namespace XmpParser
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml;
    using System.Xml.XPath;

    /// <summary>
    /// Utility classes for helping with XMP parsing.
    /// </summary>
    public static class XmlUtils
    {
        public static string TryGetValue(XmlElement parent, string element)
        {
            return TryGetValue(parent, element, string.Empty);
        }

        public static string TryGetValue(XmlElement parent, string element, string ns)
        {
            var el = parent.GetElementsByTagName(element, ns);
            return el.Item(0)?.Value ?? string.Empty;
        }

        public static int TryGetInt(XmlElement parent, string element, string ns)
        {
            string val = TryGetValue(parent, element, ns);
            int i;
            return int.TryParse(val, out i) ? i : default(int);
        }

        public static double TryGetDouble(XmlElement parent, string element, string ns)
        {
            string val = TryGetValue(parent, element, ns);
            double d;
            return double.TryParse(val, out d) ? d : default(double);
        }

        public static byte TryGetByte(XmlElement parent, string element, string ns)
        {
            string val = TryGetValue(parent, element, ns);
            byte b;
            return byte.TryParse(val, out b) ? b : default(byte);
        }

        public static DateTime? TryGetDateTime(XmlElement parent, string element, string ns, string format = null)
        {
            string val = TryGetValue(parent, element, ns);
            DateTime d;
            bool success;
            if (string.IsNullOrEmpty(format))
            {
                success = DateTime.TryParse(val, out d);
            }
            else
            {
                success = DateTime.TryParseExact(val, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
            }

            return success ? d : new DateTime?();
        }

        public static TEnum TryGetEnum<TEnum>(XmlElement parent, string element, string ns)
            where TEnum : struct
        {
            string val = TryGetValue(parent, element, ns);
            TEnum enu;
            return Enum.TryParse<TEnum>(val, true, out enu) ? enu : default(TEnum);
        }

        public static DateTime? GetSingleDateTimeByPath(XmlDocument xml, string xpath, XmlNamespaceManager resolver, string format = null)
        {
            var value = GetSingleValueByPath(xml, xpath, resolver);
            bool success = false;
            DateTime parsed = default(DateTime);

            if (!string.IsNullOrWhiteSpace(value))
            {
                if (string.IsNullOrEmpty(format))
                {
                    success = DateTime.TryParse(value, out parsed);
                }
                else
                {
                    success = DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed);
                }
            }

            return success ? parsed : new DateTime?();
        }

        public static int? GetSingleIntByPath(XmlDocument xml, string xpath, XmlNamespaceManager resolver)
        {
            var el = xml.SelectSingleNode(xpath, resolver);
            string value = el != null ? el.Value : null;

            int result;
            return int.TryParse(value, out result) ? (int?)result : null;
        }

        public static bool GetSingleBoolByPath(XmlDocument xml, string xpath, XmlNamespaceManager resolver)
        {
            var el = xml.SelectSingleNode(xpath, resolver);
            string value = el != null ? el.Value : null;

            bool result = false;
            return bool.TryParse(value, out result) ? result : false;
        }

        public static string GetSingleValueByPath(XmlDocument xml, string xpath, XmlNamespaceManager resolver)
        {
            var el = xml.SelectSingleNode(xpath, resolver);
            string value = el != null ? el.Value : null;
            return value;
        }

        public static IEnumerable<T> GetList<T>(XmlDocument xml, string xpath, Func<XmlElement, T> predicate, XmlNamespaceManager resolver)
        {
            var els = xml.SelectNodes(xpath, resolver);
            return els.OfType<XmlElement>().Select(predicate);
        }
    }
}
