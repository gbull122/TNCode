using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace TNCodeApp.Menu.ViewModels
{
    public class MenuViewModel : BindableBase
    {

        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public DelegateCommand<string> DataCommand { get; private set; }

        public MenuViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                regionManager.RequestNavigate("MainRegion", navigatePath);
        }
    }
}
