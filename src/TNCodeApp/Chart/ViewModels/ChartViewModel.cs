using OxyPlot;
using Prism.Mvvm;

namespace TNCodeApp.Chart.ViewModels
{
    public class ChartViewModel:BindableBase
    {

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

        public ChartViewModel()
        {

        }
    }
}
