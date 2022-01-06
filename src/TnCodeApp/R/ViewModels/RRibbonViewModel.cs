using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Text;
using System.Windows;
using TnCode.TnCodeApp.Data;
using TnCode.TnCodeApp.Progress;
using TnCode.TnCodeApp.R.Views;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class RRibbonViewModel : BindableBase
    {
        private ILogger loggerFacade;
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
        private IRService rService;
        private IProgressService progressService;
        private IDataSetsManager dataSetsManager;

        public bool IsMainRibbon => false;

        public bool isRRunning = false;

        public DelegateCommand ChartBuilderCommand { get; private set; }
        public DelegateCommand RStartCommand { get; private set; }
        public DelegateCommand RDetailsCommand { get; private set; }
        public DelegateCommand LoadRCommand { get; private set; }

        public bool IsRRunning
        {
            get => isRRunning;
            set
            {
                isRRunning = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRNotRunning()
        {
            return !isRRunning;
        }

        public RRibbonViewModel(ILogger logger,IEventAggregator eventAggr, IRegionManager regionMgr, IProgressService pService, IRService rServ, IDataSetsManager dataMgr)
        {
            loggerFacade = logger;
            eventAggregator = eventAggr;
            regionManager = regionMgr;
            rService = rServ;
            progressService = pService;
            dataSetsManager = dataMgr;

            ChartBuilderCommand = new DelegateCommand(CreateChart).ObservesCanExecute(() => IsRRunning);
            RStartCommand = new DelegateCommand(StartR, IsRNotRunning);
            RDetailsCommand = new DelegateCommand(ShowRDetails).ObservesCanExecute(() => IsRRunning);
            LoadRCommand = new DelegateCommand(LoadRData).ObservesCanExecute(() => IsRRunning);
        }

        private async void ShowRDetails()
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("TNCode is connected to R:");
            message.AppendLine("R Home : " + await rService.RHomeFromConnectedRAsync());
            message.AppendLine("R Version :" + await rService.RVersionFromConnectedRAsync());
            message.AppendLine("R Platform :" + await rService.RPlatformFromConnectedRAsync());

            MessageBox.Show(message.ToString(), "TNCode", MessageBoxButton.OK);
        }

        private async void StartR()
        {
            //TODO
            //Need Microsoft.R.Host.Broker.dll,*.deps.json,  *.exe, *.runtimeconfig.json, runtimeconfig.dev.json in install folder 
            await progressService.ExecuteAsync(rService.InitialiseAsync(), "Starting R...");

            IsRRunning = rService.IsRRunning;

            if (IsRRunning)
                LoadExisitingData();
        }

        private async void LoadExisitingData()
        {
            loggerFacade.LogInformation("Loading existing data to R");
            foreach (IDataSet dataSet in dataSetsManager.DataSets)
            {
                await progressService.ExecuteAsync(rService.DataSetToRAsDataFrameAsync, dataSet);
            }
        }

        private void CreateChart()
        {
            regionManager.AddToRegion("MainRegion", new GgplotBuilderView());
        }

        private async void LoadRData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "R Workspace (*.RData)|*.RData";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                var fileFullPath = openFileDialog.FileName;

                await progressService.ExecuteAsync(rService.LoadRWorkSpace, fileFullPath);
            }
        }
    }
}

