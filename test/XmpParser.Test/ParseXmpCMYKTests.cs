using Xunit;
using System.Xml.Linq;
using System.Xml;

namespace XmpParser.Test
{
    public class ParseXmpCMYKTests
    {
        [Fact]
        public void ParseXmpCMYK_NameValueTest()
        {
            XmpCMYK target = new XmpCMYK(CreateTestData());
            Assert.Equal("Test Colour", target.Name);
        }

        [Fact]
        public void ParseXmpCMYK_ModeValueTest()
        {
            XmpCMYK target = new XmpCMYK(CreateTestData());
            Assert.Equal("CMYK", target.Mode);
        }

        [Fact]
        public void ParseXmpCMYK_TypeValueTest()
        {
            XmpCMYK target = new XmpCMYK(CreateTestData());
            Assert.Equal(SwatchType.Process, target.Type);
        }

        [Fact]
        public void ParseXmpCMYK_CyanValueTest()
        {
            XmpCMYK target = new XmpCMYK(CreateTestData());
            Assert.Equal(0.4, target.Cyan);
        }

        [Fact]
        public void ParseXmpCMYK_MagentaValueTest()
        {
            XmpCMYK target = new XmpCMYK(CreateTestData());
            Assert.Equal(0.65, target.Magenta);
        }

        [Fact]
        public void ParseXmpCMYK_YellowValueTest()
        {
            XmpCMYK target = new XmpCMYK(CreateTestData());
            Assert.Equal(0.90, target.Yellow);
        }

        [Fact]
        public void ParseXmpCMYK_BlackValueTest()
        {
            XmpCMYK target = new XmpCMYK(CreateTestData());
            Assert.Equal(0.35, target.Black);
        }

        [Fact]
        public void ParseXmpCMYK_TintValueTest()
        {
            XmpCMYK target = new XmpCMYK(CreateTestData());
            Assert.Equal(100, target.Tint);
        }

        private static XmlElement CreateTestData()
        {
            XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
            XNamespace xmpG = "http://ns.adobe.com/xap/1.0/g/";
            XName li = rdf + "li";

            var xml = new XElement(li);
            xml.SetAttributeValue(rdf + "parseType", "Resource");
            xml.Add(new XElement(xmpG + "swatchName", "Test Colour"));
            xml.Add(new XElement(xmpG + "mode", "CMYK"));
            xml.Add(new XElement(xmpG + "type", "PROCESS"));
            xml.Add(new XElement(xmpG + "cyan", "40.000000"));
            xml.Add(new XElement(xmpG + "magenta", "65.000000"));
            xml.Add(new XElement(xmpG + "yellow", "90.000000"));
            xml.Add(new XElement(xmpG + "black", "35.000000"));
            xml.Add(new XElement(xmpG + "tint", "100"));

            return xml.ToXmlElement();
        }
    }
}
