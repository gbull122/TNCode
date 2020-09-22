using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TnCode.Core.R.Charts.Ggplot.Layer
{
    [Serializable]
    [XmlRoot("Property")]
    public class Property
    {
        private List<Option> options = new List<Option>();

        [XmlAttribute("Tag")]
        public string Tag { get; set; }


        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }

        [XmlArray("Options")]
        [XmlArrayItem("Option")]
        public List<Option> Options
        {
            get { return options; }
            set { options = value; }
        }
    }
}
