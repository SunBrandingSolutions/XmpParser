using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// A reference to another file embedded in this document.
    /// </summary>
    public class XmpReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmpReference" /> class.
        /// </summary>
        public XmpReference()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpReference" /> class.
        /// </summary>
        /// <param name="path">Reference file path</param>
        public XmpReference(string path)
        {
            Path = path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpReference" /> class.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
        public XmpReference(XmlElement xml)
        {
        }

        /// <summary>
        /// Gets the file path of this reference.
        /// </summary>
        public string Path { get; }
    }
}
