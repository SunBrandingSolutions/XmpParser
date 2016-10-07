using Xunit;
using System.Xml.Linq;

namespace XmpParser.Test
{
    public class XmpEventInfoTests
    {
        [Fact]
        public void XmpEventInfo_Constructor_PropertiesNotNull()
        {
            XmpEventInfo target = new XmpEventInfo();

            Assert.NotNull(target.Action);
            Assert.NotNull(target.Changed);
            Assert.NotNull(target.InstanceID);
            Assert.NotNull(target.SoftwareAgent);
        }

        [Fact]
        public void XmpEventInfo_Parse_StringPropertyNotNull()
        {
            var xml = new XElement("invalid").ToXmlElement();
            XmpEventInfo target = new XmpEventInfo(xml);

            Assert.NotNull(target.Action);
            Assert.NotNull(target.Changed);
            Assert.NotNull(target.InstanceID);
            Assert.NotNull(target.SoftwareAgent);
        }
    }
}
