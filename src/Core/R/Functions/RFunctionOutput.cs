using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TnCode.Core.R.Functions
{
    [Serializable]
    public class RFunctionOutput
    {
        private string name;
        private string outputType;

        [XmlAttribute("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute("Type")]
        public string OutputType
        {
            get { return outputType; }
            set { outputType = value; }
        }

        public RFunctionOutput() { }

        public RFunctionOutput(string name, string type)
        {
            this.name = name;
            outputType = type;
        }
    }
}
