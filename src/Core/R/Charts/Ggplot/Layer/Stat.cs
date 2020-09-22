using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TnCode.Core.R.Charts.Ggplot.Layer
{
    [Serializable, XmlRoot("Stat")]
    public class Stat
    {
        List<Property> properties = new List<Property>();
        List<Property> booleans = new List<Property>();
        List<Property> values = new List<Property>();

        [XmlAttribute("Name")]
        public string Name
        {
            get; set;
        }

        [XmlArray("Properties")]
        [XmlArrayItem("Property")]
        public List<Property> Properties
        {
            get { return properties; }
            set { properties = value; }
        }

        public bool ShouldSerializeProperties()
        {
            return Properties.Any();
        }

        [XmlArray("Booleans")]
        [XmlArrayItem("Property")]
        public List<Property> Booleans
        {
            get { return booleans; }
            set { booleans = value; }
        }

        public bool ShouldSerializeBooleans()
        {
            return Booleans.Any();
        }

        [XmlArray("Values")]
        [XmlArrayItem("Property")]
        public List<Property> Values
        {
            get { return values; }
            set { values = value; }
        }

        public bool ShouldSerializeValues()
        {
            return Values.Any();
        }

        public Stat()
        {

        }

        public string Command()
        {
            return string.Format("\"{0}\"", Name.ToLower());
        }

    }
}