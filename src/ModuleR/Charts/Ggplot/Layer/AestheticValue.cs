using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleR.Charts.Ggplot.Layer
{
    public class AestheticValue
    {
        public string Value { get; set; }
        public string IgnoreCase { get; set; }
        public string FormatString { get; set; }
        public bool? UseLowerCase { get; set; }
        public bool? IsFactor { get; set; }
        public bool Required { get; set; }
        public string Entry { get; set; }

        public AestheticValue(string value, bool required)
        {
            FormatString = "{0}";
            Required = required;
        }

        public string ReadValue()
        {
            var entryAsString = Entry;

            if (UseLowerCase.HasValue && UseLowerCase.Value)
                entryAsString = entryAsString.ToLower();

            entryAsString = string.Format(FormatString, entryAsString);

            if (IsFactor.HasValue && IsFactor.Value)
                entryAsString = "as.factor(" + entryAsString + ")";

            return Value + "=" + entryAsString;
        }
    }
}
