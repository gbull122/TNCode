using System.Collections.Generic;
using TNCode.Core.Data;
using TNCodeApp.R.Charts.Ggplot;

namespace TNCodeApp.Chart
{
    public class ScatterChart : IChart
    {
        private string title;
        private IList<string> data;
        private Ggplot ggplot;

        public string Title { get => title; set => title = value; }

        public IList<string> Data { get => data; set => data = value; }

        public string DataSetName
        {
            get;set;
        }

        public Ggplot Plot
        {
            get => ggplot;
            set
            {
                ggplot = value;
            }
        }


        public ScatterChart(IList<string> variableList)
        {
            Data = variableList;
        }

        public void Update()
        {
            ggplot = new Ggplot();


        }

        public bool CanPlot()
        {
            return data.Count == 2;
             //   && 
            //    (((IVariable)data[0]).Length == ((IVariable)data[1]).Length) &&
            //    ((IVariable)data[0]).Data== DataType.Numeric &&
            //    ((IVariable)data[1]).Data == DataType.Numeric;
        }
    }
}
