using System;
using System.Globalization;
using System.Linq;

namespace XmpParser
{
    /// <summary>
    /// Represents an XMP unique identifier value.
    /// </summary>
    public struct XmpID
    {
        private string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpID" /> struct.
        /// </summary>
        /// <param name="value">ID string value</param>
        public XmpID(string value)
        {
            _value = value;
            Guid = GetGuid(value);
        }

        /// <summary>
        /// Gets the GUID value of this ID.
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        /// Implicitly converts a string to an <see cref="XmpID" />.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>An <see cref="XmpID" /> object.</returns>
        public static implicit operator XmpID(string str) => new XmpID(str);

        /// <summary>
        /// Implicitly converts an <see cref="XmpID" /> to a string.
        /// </summary>
        /// <param name="id"><see cref="XmpID" /> to convert</param>
        /// <returns>The XMP ID as a string.</returns>
        public static implicit operator Guid(XmpID id) => id.Guid;

        /// <summary>
        /// Parses the GUID from an XMP ID string.
        /// </summary>
        /// <param name="value">XMP ID string value</param>
        /// <returns>A <see cref="Guid" /> value.</returns>
        public static Guid GetGuid(string value)
        {
            Guid guid = Guid.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                string valueToparse;

                if (value.Contains(':'))
                {
                    valueToparse = value.Substring(value.IndexOf(':') + 1);
                }
                else
                {
                    valueToparse = value;
                }

                guid = Guid.TryParse(valueToparse, out guid) ? guid : Guid.Empty;
            }

            return guid;
        }

        /// <summary>
        /// Returns the XMP ID string value.
        /// </summary>
        /// <returns>An XMP ID string value.</returns>
        public override string ToString() => _value;
    }
}
