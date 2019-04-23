using CommonServiceLocator;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows;
using TNCode.Core.Data;
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

            regionManager.RegisterViewWithRegion("RibbonRegion", typeof(DataRibbonView));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(DataSetsView));

            regionManager.RegisterViewWithRegion("MainRegion", typeof(ProgressView));
            regionManager.RequestNavigate("MainRegion", "ProgressView");
        }

        private void ShowDataSetsView()
        {
            //var mainRegion = regionManager.Regions["MainRegion"];
            //foreach (var view in mainRegion.Views)
            //{
            //    if (view.GetType().Equals(typeof(DataSetsView)))
            //    {
            //        regionManager.RequestNavigate("MainRegion", "DataSetsView");
            //        return;
            //    }
            //}

            regionManager.RegisterViewWithRegion("MainRegion", typeof(DataSetsView));
            regionManager.RequestNavigate("MainRegion", "DataSetsView");
        }

        private void ShowStatusView()
        {
            //var mainRegion = regionManager.Regions["MainRegion"];
            //foreach(var view in mainRegion.Views)
            //{
            //    if (view.GetType().Equals(typeof(ProgressView)))
            //    {
            //        regionManager.RequestNavigate("MainRegion", "ProgressView");
            //        return;
            //    }
            //}

            //regionManager.RegisterViewWithRegion("MainRegion", typeof(ProgressView));
            regionManager.RequestNavigate("MainRegion", "ProgressView");
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
