using System.Xml;

namespace XmpParser
{
    public class XmpReference
    {
        public XmpReference()
        {
        }

        public XmpReference(string path)
        {
            Path = path;
        }

        public XmpReference(XmlElement xml)
        {
        }

        public string Path { get; }
    }
}
