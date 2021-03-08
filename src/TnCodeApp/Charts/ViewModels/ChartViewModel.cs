using OxyPlot;
using Prism.Mvvm;
using TnCode.TnCodeApp.Docking;

namespace TnCode.TnCodeApp.Charts.ViewModels
{
    public class ChartViewModel : BindableBase, IDockingPanel
    {
        private IChartService chartService;

        public string Title { get; set; }

        private Model plotModel;

        public Model PlotModel
        {
            get { return plotModel; }
            set
            {
                plotModel = value;
                RaisePropertyChanged("PlotModel");
            }
        }

        public DockingMethod Docking => DockingMethod.Document;

        public ChartViewModel(IChartService cService)
        {
            chartService = cService;

            var chart = cService.GetLastChart();
            chart.Update();
            Title = chart.Title;
            PlotModel = chart.Model;
        }
    }
}