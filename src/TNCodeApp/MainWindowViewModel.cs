using Catel.MVVM;
using Microsoft.R.Host.Client;
using System;
using System.Windows;
using TNCode.Core.Data;
using TNCodeApp.Data;
using TNCodeApp.Data.Views;
using TNCodeApp.R;
using TNCodeApp.R.Views;

namespace TNCodeApp
{
    public class MainWindowViewModel : ViewModelBase
    {

        //private IRegionManager regionManager;
        //private IEventAggregator eventAggregator;
        //private IUnityContainer container;
        private IRManager rManager;
        private string title = "TNCode";
        private string statusMessage = "Ready";

        //public DelegateCommand AboutCommand { get; private set; }
        //public DelegateCommand CloseCommand { get; private set; }
        //public DelegateCommand SettingsCommand { get; private set; }
        //public DelegateCommand ShowDataSetsCommand { get; private set; }
        //public DelegateCommand ShowStatusCommand { get; private set; }

        public string Title
        {
            get { return title; }
            set {
                title = value;
                RaisePropertyChanged(nameof(Title)); }
        }

        public string StatusMessage
        {
            get { return statusMessage; }
            set {
                statusMessage = value;
                RaisePropertyChanged(nameof(StatusMessage));
            }
        }

        public MainWindowViewModel()
        {
            //container = contain;
            //regionManager = regManager;

            //eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            //CloseCommand = new DelegateCommand(Close);
            //AboutCommand = new DelegateCommand(About);
            //SettingsCommand = new DelegateCommand(Settings);
            //ShowStatusCommand = new DelegateCommand(ShowStatusView);
            //ShowDataSetsCommand = new DelegateCommand(ShowDataSetsView);

            //container.RegisterType<IXmlConverter, XmlConverter>();

            IDataSetsManager dataSetsManager = new DataSetsManager();
            //container.RegisterInstance<IDataSetsManager>(dataSetsManager);

            //IChartManager chartManager = new ChartManager(eventAggregator);
            //container.RegisterInstance<IChartManager>(chartManager);

            //rManager = new RManager(new RHostSessionCallback(), loggerFacade);
            //container.RegisterInstance<IRManager>(rManager);

            //regionManager.RegisterViewWithRegion("RibbonRegion", typeof(DataRibbonView));
            //regionManager.RegisterViewWithRegion("RibbonRegion", typeof(ChartRibbonView));
            //regionManager.RegisterViewWithRegion("RibbonRegion", typeof(RibbonRView));

            //regionManager.RegisterViewWithRegion("MainRegion", typeof(DataSetsView));

        }

        
        private void ShowDataSetsView()
        {
            //var region = regionManager.Regions["MainRegion"];

            //foreach (var view in region.Views)
            //{
            //    Type viewType = view.GetType();
            //    if (viewType.Equals(typeof(DataSetsView)))
            //    {
            //        var actualView = (DataSetsView)view;

            //        if(!actualView.IsVisible)
            //        {
            //            region.Remove(view);
            //            regionManager.AddToRegion("MainRegion", new DataSetsView());
            //        }
            //        regionManager.RequestNavigate("MainRegion", "DataSetsView");
            //    }
            //}
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
