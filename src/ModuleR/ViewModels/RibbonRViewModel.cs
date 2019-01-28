using System;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using TNCodeApp.Menu;

namespace ModuleR.ViewModels
{
    public class RibbonRViewModel : BindableBase, ITnRibbon
    {
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;

        public bool IsMainRibbon => false;

        public DelegateCommand ChartCommand { get; private set; }

        public RibbonRViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;

            ChartCommand = new DelegateCommand(CreateChart);
        }

        private void CreateChart()
        {
            var navigationParameters = new NavigationParameters();

            regionManager.RequestNavigate("MainRegion",
                new Uri("GgplotView" + navigationParameters.ToString(), UriKind.Relative));
        }
    }
}
