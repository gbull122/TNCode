using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using TnCode.TnCodeApp.Progress;
using TnCode.TnCodeApp.R.Views;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class RRibbonViewModel:BindableBase
    {
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
        private IRService rService;
        private IProgressService progressService;

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

        public bool IsRNotRunning()
        {
            return !isRRunning;
        }

        public RRibbonViewModel(IEventAggregator eventAggr, IRegionManager regionMgr, IRService rMgr, IProgressService pService)
        {
            eventAggregator = eventAggr;
            regionManager = regionMgr;
            rService = rMgr;
            progressService = pService;

            ChartBuilderCommand = new DelegateCommand(CreateChart).ObservesCanExecute(() => IsRRunning);
            RStartCommand = new DelegateCommand(StartR, IsRNotRunning);
            RDetailsCommand = new DelegateCommand(ShowRDetails).ObservesCanExecute(() => IsRRunning);

            StartR();
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
            await progressService.ExecuteAsync(rService.InitialiseAsync(), "Starting R...");

            IsRRunning = rService.IsRRunning;
        }

        private void CreateChart()
        {
            regionManager.AddToRegion("MainRegion", new GgplotBuilderView());
        }
    }
}

