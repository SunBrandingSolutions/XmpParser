using System;
using System.Xml;

namespace XmpParser
{
    /// <summary>
    /// Provides information about an event in the history of a document.
    /// </summary>
    public class XmpEventInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmpEventInfo" /> class.
        /// </summary>
        public XmpEventInfo()
        {
            Action = string.Empty;
            Changed = string.Empty;
            SoftwareAgent = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpEventInfo" /> class.
        /// </summary>
        /// <param name="action">Action that occurred</param>
        /// <param name="when">Time the action happened</param>
        public XmpEventInfo(string action, DateTime when)
        {
            Action = action;
            When = when;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmpEventInfo" /> class.
        /// </summary>
        /// <param name="xml">XML element to read from</param>
        public XmpEventInfo(XmlElement xml)
        {
            const string stEvt = "http://ns.adobe.com/xap/1.0/sType/ResourceEvent#";

            Action = XmlUtils.TryGetValue(xml, "action", stEvt);
            When = XmlUtils.TryGetDateTime(xml, "when", stEvt) ?? default(DateTime);
            InstanceID = XmlUtils.TryGetValue(xml, "instanceID", stEvt);
            SoftwareAgent = XmlUtils.TryGetValue(xml, "softwareAgent", stEvt);
            Changed = XmlUtils.TryGetValue(xml, "changed", stEvt);
        }

        /// <summary>
        /// Gets the action that occurred.
        /// </summary>
        public string Action { get; }

        /// <summary>
        /// Gets the time that the action occurred.
        /// </summary>
        public DateTime When { get; }

        /// <summary>
        /// Gets the ID of the instance.
        /// </summary>
        public XmpID InstanceID { get; }

        /// <summary>
        /// Gets the name of the software agent that performed the action.
        /// </summary>
        public string SoftwareAgent { get; }

        /// <summary>
        /// Gets the changes that were made.
        /// </summary>
        public string Changed { get; }
    }
}
