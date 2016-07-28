using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace XmpParser
{
    public static class XmpUtils
    {
        public static IEnumerable<XmlDocument> RipXmp(Stream fileStream)
        {
            var xdocs = new List<XmlDocument>();

            string filecontents;
            using (var reader = new StreamReader(fileStream))
            {
                filecontents = reader.ReadToEnd();
            }

            if (!string.IsNullOrWhiteSpace(filecontents))
            {
                int startpos = 0;
                int endpos = 0;
                string xpacket;
                XmlDocument xdoc;

                while (startpos >= 0)
                {
                    // TODO: we can use the Byte Order Mark to determine encoding
                    //       if it becomes a problem (see MediaWiki's implementation
                    //       or, if you feel brave, Adobe's specs)
                    startpos = filecontents.IndexOf("<x:xmpmeta", startpos);
                    if (startpos > 0)
                    {
                        endpos = filecontents.IndexOf("</x:xmpmeta>", startpos);

                        xpacket = filecontents.Substring(startpos, (endpos + 12) - startpos);
                        xdoc = new XmlDocument();
                        xdoc.CreateXmlDeclaration("1.0", Encoding.UTF8.ToString(), "yes");
                        xdoc.LoadXml(xpacket);
                        xdocs.Add(xdoc);

                        startpos = endpos + 12;
                    }
                }
            }

            return xdocs;
        }

        public static string RgbToHex(byte r, byte g, byte b) => $"{r:X2}{g:X2}{b:X2}";
    }
}
