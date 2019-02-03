using ModuleR.R;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using TNCodeApp.Docking;

namespace ModuleR.ViewModels
{
    public class ChartBuilderViewModel : BindableBase, ITnPanel, INavigationAware
    {
        private IRManager rManager;
        private IEventAggregator eventAggregator;

        public string Title => "Plot";

        public DockingMethod Docking => DockingMethod.Document;

        public ChartBuilderViewModel(IEventAggregator eventAggregator, IRManager rMngr)
        {
            rManager = rMngr;
            this.eventAggregator = eventAggregator;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext == null)
               return;
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
