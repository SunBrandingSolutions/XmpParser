using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// A swatch group, consisting of many <see cref="XmpSwatch" /> objects.
    /// </summary>
    public class XmpSwatchGroup : IEnumerable<XmpSwatch>
    {
        private List<XmpSwatch> swatches = new List<XmpSwatch>();

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpSwatchGroup" /> class.
        /// </summary>
        public XmpSwatchGroup()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpSwatchGroup" /> class.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
        public XmpSwatchGroup(XmlElement xml)
        {
            const string xmpG = "http://ns.adobe.com/xap/1.0/g/";
            const string rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

            GroupName = XmlUtils.TryGetValue(xml, "groupName", xmpG);
            GroupType = XmlUtils.TryGetValue(xml, "groupType", xmpG);

            var els = xml.GetElementsByTagName("Colorants", xmpG).OfType<XmlElement>();
            if (els.Any())
            {
                var xe = els.FirstOrDefault();
                var swatchEls = xe.ChildNodes.OfType<XmlElement>().Where(e => e.Name == "li" && e.NamespaceURI == rdf);
                if (swatchEls.Any())
                {
                    swatches.AddRange(swatchEls.Select(x => XmpSwatch.Create(x)).Where(x => x != null));
                }
            }
        }

        /// <summary>
        /// Gets the name of the swatch group.
        /// </summary>
        public string GroupName { get; }

        /// <summary>
        /// Gets the type of the swatch group.
        /// </summary>
        public string GroupType { get; }

        /// <summary>
        /// Gets the colorants in this group.
        /// </summary>
        public IEnumerable<XmpSwatch> Colorants
        {
            get { return swatches; }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<XmpSwatch> GetEnumerator()
        {
            return swatches.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < swatches.Count; i++)
            {
                yield return swatches[i];
            }
        }
    }
}
