﻿using System;
using System.Xml.Serialization;

namespace ModuleR.Charts.Ggplot.Layer
{
    [Serializable]
    [XmlRoot("AestheticValue")]
    public class AestheticValue
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        public string FormatString { get; set; }

        public bool? UseLowerCase { get; set; }

        public bool? IsFactor { get; set; }

        [XmlAttribute("Required")]
        public bool Required { get; set; }

        public string Entry { get; set; }

        public AestheticValue()
        {
            FormatString = "{0}";
            UseLowerCase = true;
        }

        public string ReadValue()
        {
            if (string.IsNullOrEmpty(Name))
                return string.Empty;

            var entryAsString = Entry;

            if (UseLowerCase.HasValue && UseLowerCase.Value)
                entryAsString = entryAsString.ToLower();

            entryAsString = string.Format(FormatString, entryAsString);

            if (IsFactor.HasValue && IsFactor.Value)
                entryAsString = "as.factor(" + entryAsString + ")";

            return Name + "=" + entryAsString;
        }
    }
}
