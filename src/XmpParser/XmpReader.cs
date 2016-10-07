using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// Reads XMP metadata from files.
    /// </summary>
    public static class XmpReader
    {
        /// <summary>
        /// Reads the XMP metadata as an array of XML documents
        /// </summary>
        /// <param name="fileStream">File stream to read</param>
        /// <returns>An enumeration of <see cref="XmlDocument"/> objects containing the XMP metadata.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileStream"/> cannot be null</exception>
        public static IList<XmlDocument> ReadXmp(Stream fileStream)
        {
            const string XmpStart = "<x:xmpmeta";
            const string XmpEnd = "</x:xmpmeta>";

            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream), "File stream input cannot be null");
            }

            string filecontents;
            using (var reader = new StreamReader(fileStream))
            {
                filecontents = reader.ReadToEnd();
            }

            int startpos = 0;
            int endpos = 0;

            var docs = new List<XmlDocument>();

            while (startpos >= 0)
            {
                // TODO: we can use the Byte Order Mark to determine encoding
                //       if it becomes a problem (see MediaWiki's implementation
                //       or, if you feel brave, Adobe's specs)
                startpos = filecontents?.IndexOf(XmpStart, startpos) ?? 0;
                if (startpos > 0)
                {
                    endpos = filecontents.IndexOf(XmpEnd, startpos);

                    var xpacket = filecontents.Substring(startpos, (endpos + 12) - startpos);
                    var xdoc = new XmlDocument();
                    xdoc.CreateXmlDeclaration("1.0", Encoding.UTF8.ToString(), "yes");
                    xdoc.LoadXml(xpacket);
                    docs.Add(xdoc);

                    startpos = endpos + 12;
                }
            }

            return docs;
        }
    }
}
