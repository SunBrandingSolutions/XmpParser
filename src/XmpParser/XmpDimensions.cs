using System.Linq;
using System.Xml;
using System.Xml.XPath;

namespace XmpParser
{
    public class XmpDimensions
    {
        public XmpDimensions()
        {
        }

        public XmpDimensions(XmlDocument xml, string pathToElement, XmlNamespaceManager resolver)
            : this(xml.SelectSingleNode(pathToElement, resolver) as XmlElement)
        {
        }

        public XmpDimensions(XmlElement xml)
        {
            const string stDim = "http://ns.adobe.com/xap/1.0/sType/Dimensions#";

            if (xml != null)
            {
                Width = XmlUtils.TryGetDouble(xml, "w", stDim);
                Height = XmlUtils.TryGetDouble(xml, "h", stDim);
                Unit = XmlUtils.TryGetValue(xml, "unit", stDim);
            }
        }

        public double Width { get; }

        public double Height { get; }

        public string Unit { get; }
    }
}
