using Xunit;
using System.Xml.Linq;
using System.Xml;

namespace XmpParser.Test
{
    public class ParseXmpRGBTests
    {
        [Fact]
        public void ParseXmpRGB_NameValueTest()
        {
            XmpRGB target = new XmpRGB(CreateTestData());
            Assert.Equal("Lemonchiffon", target.Name);
        }

        [Fact]
        public void ParseXmpRGB_ModeValueTest()
        {
            XmpRGB target = new XmpRGB(CreateTestData());
            Assert.Equal("RGB", target.Mode);
        }

        [Fact]
        public void ParseXmpRGB_TypeValueTest()
        {
            XmpRGB target = new XmpRGB(CreateTestData());
            Assert.Equal(SwatchType.Process, target.Type);
        }

        [Fact]
        public void ParseXmpRGB_RedValueTest()
        {
            XmpRGB target = new XmpRGB(CreateTestData());
            Assert.Equal(255, target.Red);
        }

        [Fact]
        public void ParseXmpRGB_GreenValueTest()
        {
            XmpRGB target = new XmpRGB(CreateTestData());
            Assert.Equal(250, target.Green);
        }

        [Fact]
        public void ParseXmpRGB_BlueValueTest()
        {
            XmpRGB target = new XmpRGB(CreateTestData());
            Assert.Equal(205, target.Blue);
        }

        private static XmlElement CreateTestData()
        {
            XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
            XNamespace xmpG = "http://ns.adobe.com/xap/1.0/g/";
            XName li = rdf + "li";

            var xml = new XElement(li);
            xml.SetAttributeValue(rdf + "parseType", "Resource");
            xml.Add(new XElement(xmpG + "swatchName", "Lemonchiffon"));
            xml.Add(new XElement(xmpG + "mode", "RGB"));
            xml.Add(new XElement(xmpG + "type", "PROCESS"));
            xml.Add(new XElement(xmpG + "red", "255"));
            xml.Add(new XElement(xmpG + "green", "250"));
            xml.Add(new XElement(xmpG + "blue", "205"));

            return xml.ToXmlElement(); ;
        }
    }
}
