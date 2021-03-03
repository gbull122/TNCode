using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TnCode.Core.R.Functions
{
    [Serializable]
    public class RFunctionInput
    {
        private string name;
        private List<string> columns;
        private string inputType;

        [XmlAttribute("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute("Type")]
        public string InputType
        {
            get { return inputType; }
            set { inputType = value; }
        }

        [XmlArray("Columns")]
        [XmlArrayItem("Column")]
        public List<string> Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        public bool ShouldSerializeColumns()
        {
            return columns.Any();
        }

        public RFunctionInput() { }

        public RFunctionInput(string name, List<string> columns, string type)
        {
            this.name = name;
            this.columns = columns;
            this.inputType = type;
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            RFunctionInput p = obj as RFunctionInput;
            if (p == null)
            {
                return false;
            }

            if (p.Name.Equals(this.name) && p.InputType.Equals(this.InputType))
            {
                if (p.columns.Count == this.columns.Count)
                {
                    for (int col = 0; col < p.columns.Count; col++)
                    {
                        if (!p.Columns[col].Equals(this.columns[col]))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}
