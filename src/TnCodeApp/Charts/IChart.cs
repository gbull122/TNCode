using OxyPlot;
using System.Collections.Generic;

namespace TnCode.TnCodeApp.Charts
{
    public interface IChart
    {
        string Title { get; set; }
        IList<object> Data { get; set; }
        PlotModel Model { get; set; }

        void Update();

        bool CanPlot();
    }
}
