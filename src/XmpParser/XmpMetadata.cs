using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace XmpParser
{
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

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpMetadata" /> class.
        /// </summary>
        /// <param name="xml">XML document to read from</param>
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

        /// <summary>
        /// Gets the inner XML.
        /// </summary>
        public XmlDocument InnerXml => _xml;

        /// <summary>
        /// Gets the document title.
        /// </summary>
        public string Title => _title.Value;

        /// <summary>
        /// Gets the document creator name.
        /// </summary>
        public string Creator => _creator.Value;

        /// <summary>
        /// Gets the time that this document was created.
        /// </summary>
        public DateTime? Created => _created.Value;

        /// <summary>
        /// Gets the time that this document was modified.
        /// </summary>
        public DateTime? Modified => _modified.Value;

        /// <summary>
        /// Gets the number of pages in this document..
        /// </summary>
        public int? NumPages => _numPages.Value;

        /// <summary>
        /// Gets a value indicating whether this document has a visible overprint (e.g. a varnish).
        /// </summary>
        public bool HasVisibleOverprint => _hasVisibleOverprint.Value;

        /// <summary>
        /// Gets a value indicating whether this document has a visible transparency.
        /// </summary>
        public bool HasVisibleTransparency => _hasVisibleTransparency.Value;

        /// <summary>
        /// Gets the document identifier.
        /// </summary>
        public XmpID DocumentID => _documentID.Value;

        /// <summary>
        /// Gets the identifier of the document used to create this one, if applicable.
        /// </summary>
        public XmpID OriginalDocumentID => _originalDocumentID.Value;

        /// <summary>
        /// Gets the identifier of this instance of the document.
        /// </summary>
        public XmpID InstanceID => _instanceID.Value;

        /// <summary>
        /// Gets the rendition class.
        /// </summary>
        public string RenditionClass => _renditionClass.Value;

        /// <summary>
        /// Gets the identifier of the derived document.
        /// </summary>
        public XmpID DerivedFromDocumentID => _derivedFromDocumentID.Value;

        /// <summary>
        /// Gets the identifier of the original derived document.
        /// </summary>
        public XmpID DerivedFromOriginalDocumentID => _derivedFromOriginalDocumentID.Value;

        /// <summary>
        /// Gets the identifier of the derived instance document.
        /// </summary>
        public XmpID DerivedFromInstanceID => _derivedFromInstanceID.Value;

        /// <summary>
        /// Gets the rendition class of the derived document.
        /// </summary>
        public string DerivedFromRenditionClass => _derivedFromRenditionClass.Value;

        /// <summary>
        /// Gets the name of the application used to create this document.
        /// </summary>
        public string CreatorTool => _creatorTool.Value;

        /// <summary>
        /// Gets the maximum page size within this document.
        /// </summary>
        public XmpDimensions MaxPageSize => _maxPageSize.Value;

        /// <summary>
        /// Gets all fonts in this document.
        /// </summary>
        public IEnumerable<XmpFontInfo> Fonts => _fonts.Value;

        /// <summary>
        /// Gets all history entries in this document.
        /// </summary>
        public IEnumerable<XmpEventInfo> History => _history.Value;

        /// <summary>
        /// Gets all swatches in this document.
        /// </summary>
        public IEnumerable<XmpSwatchGroup> Swatches => _swatches.Value;

        /// <summary>
        /// Gets the names of all plates or separations in this document.
        /// </summary>
        public IEnumerable<string> PlateNames => _plateNames.Value;

        /// <summary>
        /// Loads <see cref="XmpMetadata" /> from a file stream.
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <returns>An <see cref="XmpMetadata" /> object, or <c>null</c> if none
        /// can be read from the stream.</returns>
        /// <exception cref="ArgumentNullException">Input stream cannot be null</exception>
        public static XmpMetadata Load(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            var xml = XmpReader.ReadXmp(inputStream);
            if (xml.Count > 0)
            {
                return new XmpMetadata(xml[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the first color in a given plate.
        /// </summary>
        /// <param name="plateName">The plate name.</param>
        /// <returns>An <see cref="XmpSwatch" /> object, or <c>null</c></returns>
        public XmpSwatch FindColor(string plateName)
        {
            var colors = FindColors(plateName);
            if (colors.Count > 0)
            {
                return colors[0];
            }

            return null;
        }

        /// <summary>
        /// Finds swatches for a given plate.
        /// </summary>
        /// <param name="plateName">The plate name.</param>
        /// <returns>A collection of <see cref="XmpSwatch" /> object, or <c>null</c></returns>
        public IList<XmpSwatch> FindColors(string plateName)
        {
            var results = new List<XmpSwatch>();

            foreach (var swatch in Swatches)
            {
                foreach (var colorant in swatch.Colorants)
                {
                    if (string.Equals(plateName, colorant.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(colorant);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Finds a font by name in this document.
        /// </summary>
        /// <param name="name">Font name, family or face</param>
        /// <returns>An <see cref="XmpFontInfo" /> object, or <c>null</c></returns>
        public XmpFontInfo FindFont(string name)
        {
            var fonts = FindFonts(name);
            if (fonts.Count > 0)
            {
                return fonts[0];
            }

            return null;
        }

        /// <summary>
        /// Finds fonts by name in this document.
        /// </summary>
        /// <param name="name">Font name, family or face</param>
        /// <returns>A collection of <see cref="XmpFontInfo" /> object, or <c>null</c></returns>
        public IList<XmpFontInfo> FindFonts(string name)
        {
            var results = new List<XmpFontInfo>();

            foreach (var font in Fonts)
            {
                if (string.Equals(name, font.Name, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(name, font.FileName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(name, font.Face, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(font);
                }
            }

            return results;
        }

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
            foreach (var el in _xml.SelectNodes(xpath, resolver))
            {
                if (el is XmlElement)
                {
                    var transformed = predicate((XmlElement)el);
                    if (transformed != null)
                    {
                        yield return transformed;
                    }
                }
            }
        }
    }
}
