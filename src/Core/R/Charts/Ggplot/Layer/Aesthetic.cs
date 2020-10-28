using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TnCode.Core.R.Charts.Ggplot.Layer
{
    [Serializable]
    [XmlRoot("Aesthetic")]
    public class Aesthetic
    {
        List<Property> properties = new List<Property>();
        List<Property> booleans = new List<Property>();
        List<Property> values = new List<Property>();

        [XmlArray("AestheticValues")]
        [XmlArrayItem("AestheticValue")]
        public List<AestheticValue> AestheticValues
        {
            get;
            set;
        } = new List<AestheticValue>();

        [XmlElement("DefaultStat")]
        public string DefaultStat { get; set; }

        [XmlElement("DefaultPosition")]
        public string DefaultPosition { get; set; }

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
        public Aesthetic()
        {

        }

        public string Command()
        {
            var command = new StringBuilder();
            command.Append("aes(");
           
            var aesCommands = new List<string>();
            foreach (var aes in AestheticValues)
            {
                var commandString = aes.ReadValue();
                if(!string.IsNullOrEmpty(commandString))
                    aesCommands.Add(commandString);
            }
            command.Append(string.Join(",", aesCommands.ToArray()));
            command.Append(")");
            return command.ToString();
        }

        public AestheticValue GetAestheticValueByName(string name)
        {
            foreach(var aesValue in AestheticValues)
            {
                if (aesValue.Name.Equals(name))
                    return aesValue;
            }
            return null;
        }

        public bool DoesAestheticContainValue(string name)
        {
            foreach (var aesValue in AestheticValues)
            {
                if (aesValue.Name.Equals(name))
                    return true;
            }
            return false;
        }

        public void RemoveAestheticValue(string name)
        {
            foreach (var aesValue in AestheticValues)
            {
                if (aesValue.Name.Equals(name))
                    AestheticValues.Remove(aesValue);
            }
        }

        public List<Parameter> GetParameters()
        {
            var parameters = new List<Parameter>();



            return parameters;
        }
    }
}
