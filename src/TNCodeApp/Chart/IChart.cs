using OxyPlot;
using System.Collections.Generic;
using TNCode.Core.Data;

namespace TNCodeApp.Chart
{
    public interface IChart
    {
        string Title { get; set; }
        IList<IVariable> Data { get; set; }
        PlotModel Model { get; set; }

        void Update();

        bool CanPlot();
    }
}
