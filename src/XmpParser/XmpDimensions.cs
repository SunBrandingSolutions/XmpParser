using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// Defines physical dimensions.
    /// </summary>
    public class XmpDimensions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmpDimensions" /> class.
        /// </summary>
        public XmpDimensions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpDimensions" /> class.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
        /// <param name="pathToElement">XPath to the element to read</param>
        /// <param name="resolver">XML namespace resolver</param>
        public XmpDimensions(XmlDocument xml, string pathToElement, XmlNamespaceManager resolver)
            : this(xml.SelectSingleNode(pathToElement, resolver) as XmlElement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpDimensions" /> class.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
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

        /// <summary>
        /// Gets the width.
        /// </summary>
        public double Width { get; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// Gets the unit of measure.
        /// </summary>
        public string Unit { get; }
    }
}
