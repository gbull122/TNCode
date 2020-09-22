using System;
using System.Xml.Serialization;

namespace TnCode.Core.R.Charts.Ggplot.Layer
{
    [Serializable]
    [XmlRoot("Option")]
    public class Option
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }
    }
}
