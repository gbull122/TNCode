using OxyPlot;
using Prism.Mvvm;
using Prism.Regions;
using TNCodeApp.Docking;

namespace TNCodeApp.Chart.ViewModels
{
    public class ChartViewModel:BindableBase, ITnPanel, INavigationAware
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
