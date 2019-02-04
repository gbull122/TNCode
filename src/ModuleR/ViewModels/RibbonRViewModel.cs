using System;
using System.Threading.Tasks;
using ModuleR.R;
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
        private IRManager rManager;

        public bool IsMainRibbon => false;

        public DelegateCommand ChartBuilderCommand { get; private set; }
        public DelegateCommand RStartCommand { get; private set; }
        public DelegateCommand RDetailsCommand { get; private set; }

        public RibbonRViewModel(IEventAggregator eventAggr, IRegionManager regionMgr, IRManager rMgr)
        {
            eventAggregator = eventAggr;
            regionManager = regionMgr;
            rManager = rMgr;

            ChartBuilderCommand = new DelegateCommand(CreateChart, CanExecuteR);
            RStartCommand = new DelegateCommand(StartR);
            RDetailsCommand = new DelegateCommand(ShowRDetails, CanExecuteR);
        }

        private void ShowRDetails()
        {
            throw new NotImplementedException();
        }

        private async void StartR()
        {
            await rManager.InitialiseAsync();
            
            RaisePropertyChanged();
        }

        private bool CanExecuteR()
        {
            return rManager.IsRRunning;
        }

        private void CreateChart()
        {
            var navigationParameters = new NavigationParameters();

            regionManager.RequestNavigate("MainRegion",
                new Uri("ChartBuilderView" + navigationParameters.ToString(), UriKind.Relative));
        }
    }
}
