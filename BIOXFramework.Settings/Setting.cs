using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace BIOXFramework.Settings
{
    [Serializable, XmlRoot(ElementName = "settings", DataType = "string", IsNullable = true)]
    internal class RootSettings
    {
        [XmlArray("settings"), XmlArrayItem("setting", typeof(Setting))]
        public List<Setting> Settings { get; set; }
    }

    [Serializable]
    internal class Setting
    {
        [XmlAttribute("name", typeof(String))]
        public string Name { get; set; }
        [XmlAttribute("value", typeof(Object))]
        public object Value { get; set; }
    }
}