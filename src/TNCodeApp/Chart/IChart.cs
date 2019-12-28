using System.Collections.Generic;
using TNCode.Core.Data;

namespace TNCodeApp.Chart
{
    public interface IChart
    {
        string Title { get; set; }

        string DataSetName { get; set; }
        IList<string> Data { get; set; }

        void Update();

        bool CanPlot();
    }
}
