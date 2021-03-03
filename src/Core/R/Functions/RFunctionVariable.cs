using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TnCode.Core.R.Functions
{
    [Serializable]
    public class RFunctionVariable
    {
        private string name;
        private string controlType;
        private List<object> values;
        private string label;
        private string tooltip;

        [XmlAttribute("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute("ControlType")]
        public string ControlType
        {
            get { return controlType; }
            set { controlType = value; }
        }

        [XmlAttribute("Label")]
        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        [XmlAttribute("Tooltip")]
        public string Tooltip
        {
            get { return tooltip; }
            set { tooltip = value; }
        }

        [XmlArray("Values")]
        [XmlArrayItem("Value")]
        public List<object> Values
        {
            get { return values; }
            set { values = value; }
        }

        public RFunctionVariable() { }

        public RFunctionVariable(string name, string control, string label, string tooltip, List<object> value)
        {
            this.name = name;
            this.controlType = control;
            this.label = label;
            this.tooltip = tooltip;
            this.values = value;
        }
    }
}
