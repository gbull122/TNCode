using CommonServiceLocator;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows;
using TNCode.Core.Data;
using TNCodeApp.Chart.Views;
using TNCodeApp.Data;
using TNCodeApp.Data.Views;
using TNCodeApp.Progress;
using Unity;

namespace TNCodeApp
{
    public class MainWindowViewModel : BindableBase
    {

        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;
        private IUnityContainer container;
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

        public MainWindowViewModel(IUnityContainer contain, IRegionManager regManager)
        {
            container = contain;
            regionManager = regManager;

            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            CloseCommand = new DelegateCommand(Close);
            AboutCommand = new DelegateCommand(About);
            SettingsCommand = new DelegateCommand(Settings);
            ShowStatusCommand = new DelegateCommand(ShowStatusView);
            ShowDataSetsCommand = new DelegateCommand(ShowDataSetsView);

            container.RegisterType<IXmlConverter, XmlConverter>();

            IDataSetsManager dataSetsManager = new DataSetsManager();
            container.RegisterInstance<IDataSetsManager>(dataSetsManager);

            regionManager.RegisterViewWithRegion("RibbonRegion", typeof(DataRibbonView));
            regionManager.RegisterViewWithRegion("RibbonRegion", typeof(ChartRibbonView));

            regionManager.RegisterViewWithRegion("MainRegion", typeof(DataSetsView));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(ProgressView));
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

                    if(!actualView.IsVisible)
                    {
                        region.Remove(view);
                        regionManager.AddToRegion("MainRegion", new DataSetsView());
                    }
                    regionManager.RequestNavigate("MainRegion", "DataSetsView");
                }
            }
        }

        private void ShowStatusView()
        {
            var region = regionManager.Regions["MainRegion"];

            foreach (var view in region.Views)
            {
                Type viewType = view.GetType();
                if (viewType.Equals(typeof(ProgressView)))
                {
                    var actualView = (ProgressView)view;

                    if (!actualView.IsVisible)
                    {
                        region.Remove(view);
                        regionManager.AddToRegion("MainRegion", new ProgressView());
                    }
                    regionManager.RequestNavigate("MainRegion", "ProgressView");
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
