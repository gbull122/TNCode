using OxyPlot;
using System;
using System.Collections.Generic;
using TnCode.TnCodeApp.Data;

namespace TnCode.TnCodeApp.Charts
{
    public class LineChart : IChart
    {
        private string title;
        private IList<IVariable> data;
        private PlotModel plotModel;

        public string Title { get => title; set => title = value; }

        public IList<IVariable> Data { get => data; set => data = value; }

        public PlotModel Model { get => plotModel; set => plotModel = value; }
        IList<IVariable> IChart.Data { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LineChart(IList<IVariable> variableList)
        {

        }

        public void Update()
        {

        }

        public bool CanPlot()
        {
            throw new NotImplementedException();
        }
    }
}