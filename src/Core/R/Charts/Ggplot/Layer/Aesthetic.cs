using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TnCode.Core.R.Charts.Ggplot.Layer
{
    [Serializable]
    [XmlRoot("Aesthetic")]
    public class Aesthetic
    {
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
    }
}
