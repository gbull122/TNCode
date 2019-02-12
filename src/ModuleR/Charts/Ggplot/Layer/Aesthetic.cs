using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ModuleR.Charts.Ggplot.Layer
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
                aesCommands.Add(aes.ReadValue());
            }
            command.Append(string.Join(",", aesCommands.ToArray()));
            command.Append(")");
            return command.ToString();
        }
    }
}
