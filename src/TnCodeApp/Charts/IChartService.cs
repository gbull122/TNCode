using System;
using System.Collections.Generic;
using System.Text;

namespace TnCode.TnCodeApp.Charts
{
    public interface IChartService
    {
        void Create(string id, params object[] args);

        IChart GetChart(string key);

        IChart GetLastChart();
    }
}
