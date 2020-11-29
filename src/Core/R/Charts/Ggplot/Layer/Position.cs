using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TnCode.Core.R.Charts.Ggplot.Layer
{
    [Serializable]
    [XmlRoot("Position")]
    public class Position
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

        public Position()
        {

        }

        public string Command()
        {
            return string.Format("\"{0}\"", Name.ToLower());
        }

        public List<Parameter> GetParameters()
        {
            var parameters = new List<Parameter>();

            foreach (var v in Values)
            {
                parameters.Add(new Parameter(v.Name, v.Value));
            }

            foreach (var b in Booleans)
            {
                parameters.Add(new Parameter(b.Name, b.Value, true));
            }

            foreach (var p in Properties)
            {
                parameters.Add(new Parameter(p.Name, p.Value, false, true));
            }

            return parameters;
        }
    }
}
