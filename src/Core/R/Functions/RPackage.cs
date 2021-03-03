using System;
using System.Xml.Serialization;

namespace TnCode.Core.R.Functions
{
    [Serializable]
    [XmlRoot("Package")]
    public class RPackage
    {
        private string name;
        private string comparison;
        private string version;

        [XmlAttribute("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute("Version")]
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        [XmlAttribute("Comparison")]
        public string Comparison
        {
            get { return comparison; }
            set { comparison = value; }
        }

        public RPackage() { }
    }
}