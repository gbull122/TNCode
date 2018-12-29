using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using TNCodeApp.Data.Events;

namespace TNCodeApp.Menu.ViewModels
{
    public class RibbonViewModel : BindableBase, ITnRibbon
    {

        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public bool IsMainRibbon => true;

        public RibbonViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
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
