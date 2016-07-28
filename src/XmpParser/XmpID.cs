namespace XmpParser
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Represents an XMP unique identifier value.
    /// </summary>
    public struct XmpID : IFormattable
    {
        private string _value;

        public XmpID(string value)
        {
            _value = value;
            Guid = GetGuid(value);
        }

        public Guid Guid { get; }

        public static implicit operator XmpID(string str) => new XmpID(str);

        public static implicit operator Guid(XmpID id) => id.Guid;

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

        public override string ToString() => _value;

        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                return _value;
            }
            else
            {
                return Guid.ToString(format);
            }
        }
    }
}
