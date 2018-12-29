using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNCodeApp.Data.Events;

namespace TNCodeApp.Data.ViewModels
{
    public class DataRibbonViewModel:BindableBase
    {

        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public DelegateCommand<string> DataCommand { get; private set; }

        public bool IsMainRibbon => true;

        public DataRibbonViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
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
