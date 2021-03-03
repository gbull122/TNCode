using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace TnCode.Core.R.Functions
{
    [Serializable]
    [XmlRoot("Function")]
    public class RFunction
    {
        private string name;
        private string code;
        private List<RFunctionVariable> listOfVariables = new List<RFunctionVariable>();
        private List<RFunctionInput> input = new List<RFunctionInput>();
        private List<RFunctionOutput> output = new List<RFunctionOutput>();
        private RFunctionModel model;
        private string tooltip;
        private string information;
        private bool isChartFunction;

        [XmlAttribute("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute("IsChartFunction")]
        public bool IsChartFunction
        {
            get { return isChartFunction; }
            set { isChartFunction = value; }
        }

        [XmlElement("Tooltip")]
        public string Tooltip
        {
            get { return tooltip; }
            set { tooltip = value; }
        }

        [XmlIgnore]
        public string Information
        {
            get { return information; }
            set { information = value; }
        }

        [XmlElement("Information")]
        public XmlCDataSection InformationCData
        {
            get { return new XmlDocument().CreateCDataSection(information); }
            set { information = value.Value; }
        }

        [XmlArray("Inputs")]
        [XmlArrayItem("Input")]
        public List<RFunctionInput> Input
        {
            get { return input; }
            set { input = value; }
        }

        public bool ShouldSerializeInput()
        {
            return input.Any();
        }

        [XmlArray("Variables")]
        [XmlArrayItem("Variable")]
        public List<RFunctionVariable> Variables
        {
            get { return listOfVariables; }
            set { listOfVariables = value; }
        }

        public bool ShouldSerializeVariables()
        {
            return listOfVariables.Any();
        }

        [XmlArray("Outputs")]
        [XmlArrayItem("Output")]
        public List<RFunctionOutput> Output
        {
            get { return output; }
            set { output = value; }
        }

        public bool ShouldSerializeOutputs()
        {
            return output.Any();
        }

        [XmlIgnore]
        public string CodeString
        {
            get { return code; }
        }

        [XmlElement("Code")]
        public XmlCDataSection Code
        {
            get { return new System.Xml.XmlDocument().CreateCDataSection(code); }
            set { code = value.Value; }
        }

        [XmlElement("Model")]
        public RFunctionModel Model
        {
            get { return model; }
            set { model = value; }
        }

        public bool ShouldSerializeModel
        {
            get { return model != null; }
        }

        public RFunction() { }

        public RFunction(string name, string code, string info, string tooltip, bool isChart)
        {
            this.name = name;
            this.code = code;
            this.tooltip = tooltip;
            this.information = info;
            this.isChartFunction = isChart;
        }
    }
}
