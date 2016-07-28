namespace XmpParser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Adobe XMP metadata.
    /// </summary>
    public class XmpMetadata
    {
        private readonly XmlDocument _xml;
        
        private readonly Lazy<string> _title;
        private readonly Lazy<string> _creator;
        private readonly Lazy<DateTime?> _created;
        private readonly Lazy<DateTime?> _modified;
        private readonly Lazy<string> _creatorTool;
        private readonly Lazy<int?> _numPages;
        private readonly Lazy<bool> _hasVisibleOverprint;
        private readonly Lazy<bool> _hasVisibleTransparency;
        private readonly Lazy<XmpID> _documentID;
        private readonly Lazy<XmpID> _originalDocumentID;
        private readonly Lazy<XmpID> _instanceID;
        private readonly Lazy<string> _renditionClass;
        private readonly Lazy<XmpID> _derivedFromDocumentID;
        private readonly Lazy<XmpID> _derivedFromOriginalDocumentID;
        private readonly Lazy<XmpID> _derivedFromInstanceID;
        private readonly Lazy<string> _derivedFromRenditionClass;
        private readonly Lazy<XmpDimensions> _maxPageSize;
        private readonly Lazy<IEnumerable<XmpFontInfo>> _fonts;
        private readonly Lazy<IEnumerable<XmpEventInfo>> _history;
        private readonly Lazy<IEnumerable<XmpSwatchGroup>> _swatches;
        private readonly Lazy<IEnumerable<string>> _plateNames;

        public XmpMetadata(XmlDocument xml)
        {
            _xml = xml;
			
            var nr = GetResolver(xml);

            // set up lazy loaded fields
            _title = new Lazy<string>(() => GetSingleValueByPath("//rdf:RDF/rdf:Description/dc:title/rdf:Alt/rdf:li", nr));
            _creator = new Lazy<string>(() => GetSingleValueByPath("//rdf:RDF/rdf:Description/dc:creator/rdf:Alt/rdf:li", nr));
            _creatorTool = new Lazy<string>(() => GetSingleValueByPath("//rdf:RDF/rdf:Description/xmp:CreatorTool", nr));
            _created = new Lazy<DateTime?>(() => GetSingleDateTimeByPath("//rdf:RDF/rdf:Description/xmp:CreateDate", nr));
            _modified = new Lazy<DateTime?>(() => GetSingleDateTimeByPath("//rdf:RDF/rdf:Description/xmp:ModifyDate", nr));
            _numPages = new Lazy<int?>(() => GetSingleIntByPath("//rdf:RDF/rdf:Description/xmpTPg:NPages", nr));
            _hasVisibleOverprint = new Lazy<bool>(() => GetSingleBoolByPath("//rdf:RDF/rdf:Description/xmpTPg:HasVisibleOverprint", nr));
            _hasVisibleTransparency = new Lazy<bool>(() => GetSingleBoolByPath("//rdf:RDF/rdf:Description/xmpTPg:HasVisibleTransparency", nr));

            _documentID = new Lazy<XmpID>(() => GetSingleGuidByPath("//rdf:RDF/rdf:Description/xmpMM:DocumentID", nr));
            _instanceID = new Lazy<XmpID>(() => GetSingleGuidByPath("//rdf:RDF/rdf:Description/xmpMM:InstanceID", nr));
            _originalDocumentID = new Lazy<XmpID>(() => GetSingleGuidByPath("//rdf:RDF/rdf:Description/xmpMM:OriginalDocumentID", nr));
            _renditionClass = new Lazy<string>(() => GetSingleValueByPath("//rdf:RDF/rdf:Description/xmpMM:RenditionClass", nr));

            _derivedFromDocumentID = new Lazy<XmpID>(() => GetSingleGuidByPath("//rdf:RDF/rdf:Description/xmpMM:DerivedFrom/xmpMM:documentID", nr));
            _derivedFromInstanceID = new Lazy<XmpID>(() => GetSingleGuidByPath("//rdf:RDF/rdf:Description/xmpMM:DerivedFrom/xmpMM:instanceID", nr));
            _derivedFromOriginalDocumentID = new Lazy<XmpID>(() => GetSingleGuidByPath("//rdf:RDF/rdf:Description/xmpMM:DerivedFrom/xmpMM:originalDocumentID", nr));
            _derivedFromRenditionClass = new Lazy<string>(() => GetSingleValueByPath("//rdf:RDF/rdf:Description/xmpMM:DerivedFrom/xmpMM:renditionClass", nr));

            _maxPageSize = new Lazy<XmpDimensions>(() => new XmpDimensions(_xml, "//rdf:RDF/rdf:Description/xmpTPg:MaxPageSize", nr));

            _fonts = new Lazy<IEnumerable<XmpFontInfo>>(() =>
            {
                return GetList("//rdf:RDF/rdf:Description/xmpTPg:Fonts/rdf:Bag/rdf:li", e => new XmpFontInfo(e), nr);
            });

            _history = new Lazy<IEnumerable<XmpEventInfo>>(() =>
            {
                return GetList("//rdf:RDF/rdf:Description/xmpMM:History/rdf:Seq/rdf:li", e => new XmpEventInfo(e), nr);
            });

            _swatches = new Lazy<IEnumerable<XmpSwatchGroup>>(() =>
            {
                return GetList("//rdf:RDF/rdf:Description/xmpTPg:SwatchGroups/rdf:Seq/rdf:li", e => new XmpSwatchGroup(e), nr);
            });

            _plateNames = new Lazy<IEnumerable<string>>(() =>
            {
                return GetList<string>("//rdf:RDF/rdf:Description/xmpTPg:PlateNames/rdf:Seq/rdf:li", e => e.Value, nr);
            });
        }

        public string Title => _title.Value;

        public string Creator => _creator.Value;

        public DateTime? Created => _created.Value;

        public DateTime? Modified => _modified.Value;

        public int? NumPages => _numPages.Value;

        public bool HasVisibleOverprint => _hasVisibleOverprint.Value;

        public bool HasVisibleTransparency => _hasVisibleTransparency.Value;

        public XmpID DocumentID => _documentID.Value;

        public XmpID OriginalDocumentID => _originalDocumentID.Value;

        public XmpID InstanceID => _instanceID.Value;

        public string RenditionClass => _renditionClass.Value;

        public XmpID DerivedFromDocumentID => _derivedFromDocumentID.Value;

        public XmpID DerivedFromOriginalDocumentID => _derivedFromOriginalDocumentID.Value;

        public XmpID DerivedFromInstanceID => _derivedFromInstanceID.Value;

        public string DerivedFromRenditionClass => _derivedFromRenditionClass.Value;

        public string CreatorTool => _creatorTool.Value;

        public XmpDimensions MaxPageSize => _maxPageSize.Value;

        public IEnumerable<XmpFontInfo> Fonts => _fonts.Value;

        public IEnumerable<XmpEventInfo> History => _history.Value;

        public IEnumerable<XmpSwatchGroup> Swatches => _swatches.Value;

        public IEnumerable<string> PlateNames => _plateNames.Value;

        public string FontList
        {
            get
            {
                StringBuilder fonts = new StringBuilder();
                foreach (var font in Fonts)
                {
                    fonts.Append(' ');
                    fonts.Append(font.Family);
                    fonts.Append(' ');
                    fonts.Append(font.Face);
                }

                return fonts.ToString().Trim();
            }
        }

        public static XmpMetadata Load(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            var xml = XmpUtils.RipXmp(inputStream);
            if (xml.Any())
            {
                return new XmpMetadata(xml.First());
            }
            else
            {
                return null;
            }
        }

        public XmpSwatch FindColor(string plateName)
        {
            return FindColors(plateName).FirstOrDefault();
        }

        public IEnumerable<XmpSwatch> FindColors(string plateName)
        {
            return from s in Swatches.SelectMany(s => s.Colorants)
                   where s.Name == plateName
                   orderby s.Name
                   select s;
        }

        public XmpFontInfo FindFont(string name)
        {
            return FindFonts(name).FirstOrDefault();
        }

        public IEnumerable<XmpFontInfo> FindFonts(string name)
        {
            return from f in Fonts
                   where f.Name == name ||
                    f.FileName == name ||
                    f.Face == name
                   orderby f.Name
                   select f;
        }
        
        public XmlDocument GetXml() => _xml;
        
        private static XmlNamespaceManager GetResolver(XmlDocument xml)
        {
            var resolver = new XmlNamespaceManager(xml.NameTable);

            // there must be a better way of doing this...
            resolver.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
            resolver.AddNamespace("illustrator", "http://ns.adobe.com/illustrator/1.0/");
            resolver.AddNamespace("pdf", "http://ns.adobe.com/pdf/1.3/");
            resolver.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            resolver.AddNamespace("stDim", "http://ns.adobe.com/xap/1.0/sType/Dimensions#");
            resolver.AddNamespace("stEvt", "http://ns.adobe.com/xap/1.0/sType/ResourceEvent#");
            resolver.AddNamespace("stFnt", "http://ns.adobe.com/xap/1.0/sType/Font#");
            resolver.AddNamespace("stRef", "http://ns.adobe.com/xap/1.0/sType/ResourceRef#");
            resolver.AddNamespace("xmp", "http://ns.adobe.com/xap/1.0/");
            resolver.AddNamespace("xmpG", "http://ns.adobe.com/xap/1.0/g/");
            resolver.AddNamespace("xmpGImg", "http://ns.adobe.com/xap/1.0/g/img/");
            resolver.AddNamespace("xmpMM", "http://ns.adobe.com/xap/1.0/mm/");
            resolver.AddNamespace("xmpTPg", "http://ns.adobe.com/xap/1.0/t/pg/");

            return resolver;
        }

        private DateTime? GetSingleDateTimeByPath(string xpath, XmlNamespaceManager resolver)
        {
            var value = GetSingleValueByPath(xpath, resolver);
            DateTime result;

            if (!string.IsNullOrWhiteSpace(value) &&
                DateTime.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        private XmpID GetSingleGuidByPath(string xpath, XmlNamespaceManager resolver)
        {
            var value = GetSingleValueByPath(xpath, resolver);
            return new XmpID(value);
        }

        private int? GetSingleIntByPath(string xpath, XmlNamespaceManager resolver)
        {
            var el = _xml.SelectSingleNode(xpath, resolver);
            string value = el != null ? el.Value : null;

            int result;
            return int.TryParse(value, out result) ? (int?)result : null;
        }

        private bool GetSingleBoolByPath(string xpath, XmlNamespaceManager resolver)
        {
            var el = _xml.SelectSingleNode(xpath, resolver);
            string value = el != null ? el.Value : null;

            bool result = false;
            return bool.TryParse(value, out result) ? result : false;
        }

        private string GetSingleValueByPath(string xpath, XmlNamespaceManager resolver)
        {
            var el = _xml.SelectSingleNode(xpath, resolver);
            string value = el != null ? el.Value : string.Empty;
            return value;
        }

        private IEnumerable<T> GetList<T>(string xpath, Func<XmlElement, T> predicate, XmlNamespaceManager resolver)
        {
            var els = _xml.SelectNodes(xpath, resolver);
            return els.OfType<XmlElement>().Select(predicate).Where(x => x != null).ToArray();
        }
    }
}
