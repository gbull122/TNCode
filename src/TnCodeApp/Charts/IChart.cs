using OxyPlot;
using System.Collections.Generic;
using TnCode.TnCodeApp.Data;

namespace TnCode.TnCodeApp.Charts
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
