using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// Utility classes for helping with XMP parsing.
    /// </summary>
    internal static class XmlUtils
    {
        public static string TryGetValue(XmlElement parent, string element)
        {
            return TryGetValue(parent, element, string.Empty);
        }

        public static string TryGetValue(XmlElement parent, string element, string ns)
        {
            var el = parent.GetElementsByTagName(element, ns);
            return el.Item(0)?.InnerText ?? string.Empty;
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
            DateTime? result = null;

            string val = TryGetValue(parent, element, ns);
            DateTime d;
            if (string.IsNullOrEmpty(format))
            {
                if (DateTime.TryParse(val, out d))
                {
                    result = d;
                }
            }
            else
            {
                if (DateTime.TryParseExact(val, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                {
                    result = d;
                }
            }

            return result;
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
            DateTime? result = null;

            var value = GetSingleValueByPath(xml, xpath, resolver);
            DateTime parsed;

            if (!string.IsNullOrWhiteSpace(value))
            {
                if (string.IsNullOrEmpty(format))
                {
                    if (DateTime.TryParse(value, out parsed))
                    {
                        result = parsed;
                    }
                }
                else
                {
                    if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                    {
                        result = parsed;
                    }
                }
            }

            return result;
        }

        public static int? GetSingleIntByPath(XmlDocument xml, string xpath, XmlNamespaceManager resolver)
        {
            var el = xml.SelectSingleNode(xpath, resolver);
            string value = el?.InnerText;

            int result;
            return int.TryParse(value, out result) ? (int?)result : null;
        }

        public static bool GetSingleBoolByPath(XmlDocument xml, string xpath, XmlNamespaceManager resolver)
        {
            var el = xml.SelectSingleNode(xpath, resolver);
            string value = el?.InnerText;

            bool result = false;
            return bool.TryParse(value, out result) ? result : false;
        }

        public static string GetSingleValueByPath(XmlDocument xml, string xpath, XmlNamespaceManager resolver)
        {
            var el = xml.SelectSingleNode(xpath, resolver);
            string value = el?.InnerText;
            return value;
        }

        public static IEnumerable<T> GetList<T>(XmlDocument xml, string xpath, Func<XmlElement, T> predicate, XmlNamespaceManager resolver)
        {
            foreach (var el in xml.SelectNodes(xpath, resolver))
            {
                if (el is XmlElement)
                {
                    yield return predicate((XmlElement)el);
                }
            }
        }
    }
}
