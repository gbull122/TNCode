using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public bool isRRunning = false;

        public DelegateCommand ChartBuilderCommand { get; private set; }
        public DelegateCommand RStartCommand { get; private set; }
        public DelegateCommand RDetailsCommand { get; private set; }

        public bool IsRRunning
        {
            get => isRRunning;
            set
            {
                isRRunning = value;
                RaisePropertyChanged();
            }
        }

        public RibbonRViewModel(IEventAggregator eventAggr, IRegionManager regionMgr, IRManager rMgr)
        {
            eventAggregator = eventAggr;
            regionManager = regionMgr;
            rManager = rMgr;

            ChartBuilderCommand = new DelegateCommand(CreateChart).ObservesCanExecute(() => IsRRunning);
            RStartCommand = new DelegateCommand(StartR);
            RDetailsCommand = new DelegateCommand(ShowRDetails).ObservesCanExecute(() => IsRRunning);
        }

        private async void ShowRDetails()
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("TNCode is connected to R:");
            message.AppendLine("R Home : " + await rManager.RHomeFromConnectedRAsync());
            message.AppendLine("R Version :" + await rManager.RVersionFromConnectedRAsync());
            message.AppendLine("R Platform :" + await rManager.RPlatformFromConnectedRAsync());

            MessageBox.Show(message.ToString(), "TNCode", MessageBoxButton.OK);
        }

        private async void StartR()
        {
            await rManager.InitialiseAsync();

            IsRRunning = rManager.IsRRunning;
        }

        private void CreateChart()
        {
            var navigationParameters = new NavigationParameters();

            regionManager.RequestNavigate("MainRegion",
                new Uri("ChartBuilderView" + navigationParameters.ToString(), UriKind.Relative));
        }
    }
}
