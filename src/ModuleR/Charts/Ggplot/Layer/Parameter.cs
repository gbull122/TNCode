using System;

namespace ModuleR.Charts.Ggplot.Layer
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool UseQuotes { get; set; }

        public Parameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Command()
        {
            if (UseQuotes)
                return Name + "=" + String.Format("\"{0}\"", Value);

            return Name + "=" + Value;
        }
    }
}
