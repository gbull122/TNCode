using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var parameters = GetParameters();

            var command = new StringBuilder();

            command.Append(string.Format("position_{0}(", Name.ToLower()));

            List<string> paramCommands = new List<string>();
            foreach(var param in parameters)
            {
                paramCommands.Add(param.Command());
            }

            command.Append(string.Join(",", paramCommands));
            command.Append(")");
            return command.ToString();
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
                if (p.MultiOptions.Count > 0)
                {
                    List<string> selected = new List<string>();
                    foreach (var o in p.MultiOptions)
                    {
                        if (o.Selected)
                            selected.Add(String.Format("\"{0}\"", o.Value));
                    }

                    var command = "c("+string.Join(",", selected)+")";
                    parameters.Add(new Parameter(p.Name, command));
                }
                else
                {
                    parameters.Add(new Parameter(p.Name, p.Value, false, true));
                }
            }

            return parameters;
        }
    }
}
