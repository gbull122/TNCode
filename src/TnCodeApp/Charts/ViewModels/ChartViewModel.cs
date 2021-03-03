using OxyPlot;
using Prism.Mvvm;
using TnCode.TnCodeApp.Docking;

namespace TnCode.TnCodeApp.Charts.ViewModels
{
    public class ChartViewModel : BindableBase, IDockingPanel
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

        public string Title => "Chart";

        public DockingMethod Docking => DockingMethod.Document;

        public ChartViewModel()
        {

        }
    }
}