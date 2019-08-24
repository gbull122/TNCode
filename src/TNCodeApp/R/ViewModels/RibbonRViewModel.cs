using Catel.MVVM;
using System.Text;
using System.Windows;
using TNCodeApp.R.Views;

namespace TNCodeApp.R.ViewModels
{
    public class RibbonRViewModel : ViewModelBase
    {
        //private IEventAggregator eventAggregator;
        //private IRegionManager regionManager;
        private IRManager rManager;
        //private IProgressService progressService;

        public bool IsMainRibbon => false;

        public bool isRRunning = false;

        //public DelegateCommand ChartBuilderCommand { get; private set; }
        //public DelegateCommand RStartCommand { get; private set; }
        //public DelegateCommand RDetailsCommand { get; private set; }

        public bool IsRRunning
        {
            get => isRRunning;
            set
            {
                isRRunning = value;
                RaisePropertyChanged(nameof(IsRRunning));
            }
        }

        public RibbonRViewModel(IRManager rMgr)
        {
            //eventAggregator = eventAggr;
            //regionManager = regionMgr;
            rManager = rMgr;
            //progressService = pService;

            //ChartBuilderCommand = new DelegateCommand(CreateChart).ObservesCanExecute(() => IsRRunning);
            //RStartCommand = new DelegateCommand(StartR);
            //RDetailsCommand = new DelegateCommand(ShowRDetails).ObservesCanExecute(() => IsRRunning);
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
            //await progressService.ExecuteAsync(rManager.InitialiseAsync(), "Starting R...");

            IsRRunning = rManager.IsRRunning;
        }

        private void CreateChart()
        {
            //regionManager.AddToRegion("MainRegion", new ChartBuilderView());

        }
    }
}
