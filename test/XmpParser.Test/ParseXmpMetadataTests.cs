using System;
using Xunit;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace XmpParser.Test
{
    public class ParseXmpMetadataTests
    {
        [Fact]
        public void ParseXmpMetadata_GetDocumentIDTest()
        {
            XmpMetadata target = LoadMetadata();

            Guid expected = new Guid("bc07cbcd-9eb4-405f-8c8f-20666219e6ca");
            Guid actual = target.DocumentID;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParseXmpMetadata_GetOriginalDocumentIDTest()
        {
            XmpMetadata target = LoadMetadata();

            Guid expected = new Guid("5d208924-93bf-db11-914a-8590d31508c8");
            Guid actual = target.OriginalDocumentID;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParseXmpMetadata_GetNumPagesTest()
        {
            XmpMetadata target = LoadMetadata();

            int expected = 1;
            int actual = target.NumPages.GetValueOrDefault();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParseXmpMetadata_GetOverprintTest()
        {
            XmpMetadata target = LoadMetadata();
            Assert.False(target.HasVisibleOverprint);
        }

        [Fact]
        public void ParseXmpMetadata_GetTransparencyTest()
        {
            XmpMetadata target = LoadMetadata();
            Assert.True(target.HasVisibleTransparency);
        }

        [Fact]
        public void ParseXmpMetadata_GetMaxPageSizeTest()
        {
            XmpMetadata target1 = LoadMetadata("XmpSample1.xml");
            XmpMetadata target2 = LoadMetadata("XmpSample2.xml");

            var test = new Func<XmpDimensions, double, double, string, bool>((x, w, h, u) =>
            {
                return x.Width == w && x.Height == h && x.Unit == u;
            });

            Assert.True(test(target1.MaxPageSize, 210.001556, 297.000083, "Millimeters"));
            Assert.True(test(target2.MaxPageSize, 210.001556, 297.000083, "Millimeters"));
        }

        private static XmpMetadata LoadMetadata(string fileName = "XmpSample1.xml")
        {
            var doc = new XmlDocument();
            doc.Load(File.OpenRead(fileName));
            return new XmpMetadata(doc);
        }
    }
}
