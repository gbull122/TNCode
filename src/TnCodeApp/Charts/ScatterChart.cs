using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Linq;
using TnCode.TnCodeApp.Data;

namespace TnCode.TnCodeApp.Charts
{
    public class ScatterChart : IChart
    {
        private string title;
        private IList<IVariable> data;
        private PlotModel plotModel;
        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChanged(string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public string Title { get => title; set => title = value; }

        public IList<IVariable> Data { get => data; set => data = value; }

        public PlotModel Model
        {
            get => plotModel;
            set
            {
                plotModel = value;
            }
        }

        public ScatterChart() { }
        public ScatterChart(IList<IVariable> variableList)
        {
            Data = variableList;
        }

        public void Update()
        {
            plotModel = new PlotModel();
            plotModel.Title = title;

            var scatterSeries = new ScatterSeries();
            scatterSeries.MarkerType = MarkerType.Circle;


            for (int row = 0; row < ((IVariable)data[0]).Length; row++)
            {

                var x = double.Parse(((IVariable)data[0]).Values.ElementAt(row).ToString());
                var y = double.Parse(((IVariable)data[1]).Values.ElementAt(row).ToString());
                var scatterPoint = new ScatterPoint(x, y);
                scatterSeries.Points.Add(scatterPoint);
            }

            plotModel.Series.Add(scatterSeries);

            Model = plotModel;
        }

        public bool CanPlot()
        {
            return data.Count == 2 &&
                (((IVariable)data[0]).Length == ((IVariable)data[1]).Length) &&
                ((IVariable)data[0]).DataFormat == Variable.Format.Continuous &&
                ((IVariable)data[1]).DataFormat == Variable.Format.Continuous;
        }
    }
}
