using Xunit;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;

namespace XmpParser.Test
{
    public class ParseXmpSwatchesTest
    {
        [Fact]
        public void ParseXmpSwatches_CreateRGBTest()
        {
            var data = CreateTestData(new Dictionary<string, string>
            {
                { "mode", "RGB" },
                { "swatchName", "Test RGB" },
                { "type", "PROCESS" },
                { "red", "255" },
                { "green", "250" },
                { "blue", "205" }
            });

            XmpSwatch target = XmpSwatch.Create(data);
            Assert.IsType<XmpRGB>(target);
        }

        [Fact]
        public void ParseXmpSwatches_CreateCMYKTest()
        {
            var data = CreateTestData(new Dictionary<string, string>
            {
                { "mode", "CMYK" },
                { "swatchName", "Test CMYK" },
                { "type", "PROCESS" },
                { "cyan", "0.000000" },
                { "magenta", "0.000000" },
                { "yellow", "0.000000" },
                { "black", "0.000000" }
            });

            XmpSwatch target = XmpSwatch.Create(data);
            Assert.IsType<XmpCMYK>(target);
        }

        [Fact]
        public void ParseXmpSwatches_CreateLABTest()
        {
            var data = CreateTestData(new Dictionary<string, string>
            {
                { "mode", "LAB" },
                { "swatchName", "Test LAB" },
                { "type", "PROCESS" },
                { "L", "100" },
                { "A", "127" },
                { "B", "127" }
            });

            XmpSwatch target = XmpSwatch.Create(data);
            Assert.IsType<XmpLAB>(target);
        }

        [Fact]
        public void ParseXmpSwatches_CreateGrayTest()
        {
            var data = CreateTestData(new Dictionary<string, string>
            {
                { "mode", "GRAY" },
                { "swatchName", "Test Grey" },
                { "gray", "255" }
            });

            XmpSwatch target = XmpSwatch.Create(data);
            Assert.IsType<XmpGray>(target);
        }

        private static XmlElement CreateTestData(IDictionary<string, string> values)
        {
            XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
            XNamespace xmpG = "http://ns.adobe.com/xap/1.0/g/";
            XName li = rdf + "li";

            var xml = new XElement(li);
            xml.SetAttributeValue(rdf + "parseType", "Resource");

            foreach (string key in values.Keys)
            {
                xml.Add(new XElement(xmpG + key, values[key]));
            }

            return xml.ToXmlElement();
        }
    }
}
