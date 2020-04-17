using System.Collections.Generic;
using System.Linq;

namespace TNCodeApp.R.Charts.Ggplot.Layer
{
    public class Stat
    {
        private List<Parameter> _params;

        public string Name { get; set; }

        public Stat()
        {
            Name = "identity";
            _params = new List<Parameter>();
        }

        public string Command()
        {
            return string.Format("\"{0}\"", Name.ToLower());
        }

        public void ClearParams()
        {
            _params.Clear();
        }

        public void AddParams(Parameter param)
        {
            if (!_params.Contains(param))
                _params.Add(param);
        }

        public void RemoveParam(Parameter param)
        {
            if (_params.Contains(param))
                _params.Remove(param);
        }

        public List<Parameter> Params()
        {
            return _params.Where(s => !string.IsNullOrWhiteSpace(s.Value)).Distinct().ToList();
        }
    }
}