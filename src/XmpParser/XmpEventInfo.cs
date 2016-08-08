namespace XmpParser
{
    using System;
    using System.Xml;

    public class XmpEventInfo
    {
        public XmpEventInfo()
        {
            Action = string.Empty;
            Changed = string.Empty;
            InstanceID = new XmpID();
            SoftwareAgent = string.Empty;
        }

        public XmpEventInfo(string action, DateTime when)
        {
            Action = action;
            When = when;
        }

        public XmpEventInfo(XmlElement xml)
        {
            const string stEvt = "http://ns.adobe.com/xap/1.0/sType/ResourceEvent#";

            Action = XmlUtils.TryGetValue(xml, "action", stEvt);
            When = XmlUtils.TryGetDateTime(xml, "when", stEvt) ?? default(DateTime);
            InstanceID = XmlUtils.TryGetValue(xml, "instanceID", stEvt);
            SoftwareAgent = XmlUtils.TryGetValue(xml, "softwareAgent", stEvt);
            Changed = XmlUtils.TryGetValue(xml, "changed", stEvt);
        }

        public string Action { get; }

        public DateTime When { get; }

        public XmpID InstanceID { get; }

        public string SoftwareAgent { get; }

        public string Changed { get; }
    }
}
