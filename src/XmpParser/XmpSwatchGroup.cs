namespace XmpParser
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class XmpSwatchGroup : IEnumerable<XmpSwatch>
    {
        private List<XmpSwatch> swatches = new List<XmpSwatch>();

        public XmpSwatchGroup()
        {
        }

        public XmpSwatchGroup(XmlElement xml)
        {
            string xmpG = "http://ns.adobe.com/xap/1.0/g/";
            string rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

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

        public string GroupName { get; }

        public string GroupType { get; }

        public IList<XmpSwatch> Colorants
        {
            get { return swatches; }
        }

        public IEnumerator<XmpSwatch> GetEnumerator()
        {
            return swatches.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < swatches.Count; i++)
            {
                yield return swatches[i];
            }
        }
    }
}
