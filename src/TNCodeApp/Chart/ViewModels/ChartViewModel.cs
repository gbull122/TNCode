using OxyPlot;
using Prism.Mvvm;
using TNCodeApp.Docking;

namespace TNCodeApp.Chart.ViewModels
{
    public class ChartViewModel:BindableBase, ITnPanel
    {

        private PlotModel plotModel;
        private IChartManager chartManager;

        public PlotModel PlotModel
        {
            get { return plotModel; }
            set
            {
                plotModel = value;
                RaisePropertyChanged("PlotModel");
            }
        }

        public string Title { get; set; }

        public DockingMethod Docking => DockingMethod.Document;

        public ChartViewModel(IChartManager chartMgr)
        {
            chartManager = chartMgr;

            var chart = chartManager.GetLastChart();
            chart.Update();
            Title = chart.Title;
            PlotModel = chart.Model;
        }
    }
}
