using System.Collections.Generic;
using TNCode.Core.Data;
using TNCodeApp.R.Charts.Ggplot.Layer;

namespace TNCodeApp.Chart
{
    public interface IChart
    {
        string Title { get; set; }

        string DataSetName { get; set; }
        Dictionary<string, ICollection<string>> Data { get; set; }

        void Update();

        bool CanPlot();
        Layer ChartLayer { get; }
        IXmlConverter Converter { set; }
    }
}
