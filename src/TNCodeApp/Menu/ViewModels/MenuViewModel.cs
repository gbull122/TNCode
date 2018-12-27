using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using TNCodeApp.Data.Events;

namespace TNCodeApp.Menu.ViewModels
{
    public class MenuViewModel : BindableBase, ITnRibbon
    {

        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public DelegateCommand<string> DataCommand { get; private set; }

        public bool IsMainRibbon => true;

        public MenuViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            NavigateCommand = new DelegateCommand<string>(Navigate);
            DataCommand = new DelegateCommand<string>(Data);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                regionManager.RequestNavigate("MainRegion", navigatePath);
        }

        private void Data(string action)
        {
            eventAggregator.GetEvent<TestDataEvent>().Publish("Mpg");
        }
    }
}
