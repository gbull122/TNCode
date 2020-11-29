using System;

namespace TnCode.Core.R.Charts.Ggplot.Layer
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool UseQuotes { get; set; }
        public bool Capitlise { get; set; }

        public Parameter(string name, string value, bool capitalise = false, bool useQuotes=false)
        {
            Name = name;
            Value = value;
            UseQuotes = useQuotes;
            Capitlise = capitalise;
        }

        public string Command()
        {
            if (UseQuotes)
                return Name + "=" + String.Format("\"{0}\"", Value);

            if(Capitlise)
                return Name + "=" + Value.ToUpper();

            return Name + "=" + Value;
        }
    }
}
