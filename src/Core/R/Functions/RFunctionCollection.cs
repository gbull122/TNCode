using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TnCode.Core.R.Functions
{
    [Serializable]
    [XmlRoot("SharpRFunctions")]
    public class RFunctionCollection
    {
        private string name;

        private List<RPackage> packages = new List<RPackage>();

        private List<RFunction> functions = new List<RFunction>();

        [XmlAttribute("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlArray("Packages")]
        public List<RPackage> Packages
        {
            get { return packages; }
            set { packages = value; }
        }

        public bool ShouldSerializePackages()
        {
            return packages.Any();
        }

        [XmlArray("Functions")]
        public List<RFunction> Functions
        {
            get { return functions; }
            set { functions = value; }
        }

        public bool ShouldSerializeFunctions()
        {
            return functions.Any();
        }

        public RFunctionCollection()
        {

        }

        public RFunction GetAFunction(string name)
        {
            foreach (var rFunc in functions)
            {
                if (rFunc.Name.Equals(name))
                    return rFunc;
            }
            return null;
        }
    }
}
