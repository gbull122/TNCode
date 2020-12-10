using System;

namespace TnCode.Core.R.Charts.Ggplot.Layer
{
    public interface IParameter
    {
        bool Capitlise { get; set; }
        string Name { get; set; }
        bool UseQuotes { get; set; }
        string Value { get; set; }

        string Command();
    }

    public class Parameter : IParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool UseQuotes { get; set; }
        public bool Capitlise { get; set; }

        public Parameter(string name, string value, bool capitalise = false, bool useQuotes = false)
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

            if (Capitlise)
                return Name + "=" + Value.ToUpper();

            return Name + "=" + Value;
        }
    }
}
