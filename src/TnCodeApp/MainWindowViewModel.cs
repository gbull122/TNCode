using CommonServiceLocator;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.IO;
using System.Windows;
using TnCode.Core.R;
using TnCode.Core.Utilities;
using TnCode.TnCodeApp.Data;
using TnCode.TnCodeApp.Data.Views;
using TnCode.TnCodeApp.Logger;
using TnCode.TnCodeApp.Progress;
using TnCode.TnCodeApp.R;
using TnCode.TnCodeApp.R.Views;
using Unity;

namespace TnCode.TnCodeApp
{
    public class MainWindowViewModel : BindableBase
    {

        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;
        private IUnityContainer container;
        private IRService rService;

        private string title = "TNCode";
        private string statusMessage = "Ready";

        public DelegateCommand AboutCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand SettingsCommand { get; private set; }
        public DelegateCommand ShowDataSetsCommand { get; private set; }
        public DelegateCommand ShowStatusCommand { get; private set; }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public string StatusMessage
        {
            get { return statusMessage; }
            set { SetProperty(ref statusMessage, value); }
        }

        public MainWindowViewModel(IUnityContainer contain, IRegionManager regManager, ILoggerFacade loggerFacade)
        {
            container = contain;
            regionManager = regManager;

            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            CloseCommand = new DelegateCommand(Close);
            AboutCommand = new DelegateCommand(About);
            SettingsCommand = new DelegateCommand(Settings);
            ShowStatusCommand = new DelegateCommand(ShowLogView);
            ShowDataSetsCommand = new DelegateCommand(ShowDataSetsView);

            IXmlConverter xmlConverter = new XmlConverter();
            container.RegisterInstance<IXmlConverter>(xmlConverter);

            IDataSetsManager dataSetsManager = new DataSetsManager();
            container.RegisterInstance<IDataSetsManager>(dataSetsManager);

            //IChartManager chartManager = new ChartManager(eventAggregator, xmlConverter, regionManager);
            //container.RegisterInstance<IChartManager>(chartManager);


            //var rManager = new RManager(tempFolder);

            //rService = new RService(loggerFacade, eventAggregator, rManager);
            //container.RegisterInstance<IRService>(rService);

            regionManager.RegisterViewWithRegion("RibbonRegion", typeof(DataRibbonView));
            //regionManager.RegisterViewWithRegion("RibbonRegion", typeof(ChartRibbonView));
            regionManager.RegisterViewWithRegion("RibbonRegion", typeof(RRibbonView));

            regionManager.RegisterViewWithRegion("MainRegion", typeof(DataSetsView));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(LoggerView));

            regionManager.RegisterViewWithRegion("StatusBarRegion", typeof(ProgressView));
        }

        private void ShowDataSetsView()
        {
            var region = regionManager.Regions["MainRegion"];

            foreach (var view in region.Views)
            {
                Type viewType = view.GetType();
                if (viewType.Equals(typeof(DataSetsView)))
                {
                    var actualView = (DataSetsView)view;

                    if (!actualView.IsVisible)
                    {
                        region.Remove(view);
                        regionManager.AddToRegion("MainRegion", new DataSetsView());
                    }
                    regionManager.RequestNavigate("MainRegion", "DataSetsView");
                }
            }
        }

        private void ShowLogView()
        {
            var region = regionManager.Regions["MainRegion"];

            foreach (var view in region.Views)
            {
                Type viewType = view.GetType();
                if (viewType.Equals(typeof(LoggerView)))
                {
                    var actualView = (LoggerView)view;

                    if (!actualView.IsVisible)
                    {
                        region.Remove(view);
                        regionManager.AddToRegion("MainRegion", new LoggerView());
                    }
                    regionManager.RequestNavigate("MainRegion", "LogView");
                }
            }
        }

        private void Settings()
        {

        }

        private void About()
        {


        }

        private void Close()
        {
            Application.Current.Shutdown();
        }
    }
}
