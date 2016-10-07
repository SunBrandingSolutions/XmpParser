namespace System.Xml.Linq
{
    public static class XElementExtensions
    {
        public static XmlElement ToXmlElement(this XElement xelement)
        {
            var doc = new XmlDocument();
            doc.Load(xelement.CreateReader());
            return doc.DocumentElement;
        }
    }
}