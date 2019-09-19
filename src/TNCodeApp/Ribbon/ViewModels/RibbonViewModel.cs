using System;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using TNCodeApp.Data;
using TNCodeApp.Data.ViewModels;
using TNCodeApp.Data.Views;
using TNCodeApp.Docking;

namespace TNCodeApp.Ribbon.ViewModels
{
    public class RibbonViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private IDockingService dockingService;

        public Command AboutCommand { get; private set; }
        public Command CloseCommand { get; private set; }
        public Command SettingsCommand { get; private set; }
        public Command ShowDataSetsCommand { get; private set; }

        public Command LoadDataCommand { get; private set; }
        public Command TestDataCommand { get; private set; }

        public RibbonViewModel(INavigationService navService, IDockingService dockService)
        {
            navigationService = navService;
            dockingService = dockService;

            TestDataCommand = new Command(LoadTestData);
            LoadDataCommand = new Command(LoadData);
        }

        private void LoadData()
        {
            throw new NotImplementedException();
        }

        private async void LoadTestData()
        {
            //var mpgData = new MpgDataSet();

            //    var dataList = new List<object[,]>() { mpgData.Data };

            //    var testData = new DataSet(dataList, "Mpg");
            //    datasetsManager.DataSets.Add(testData);

            //    //eventAggregator.GetEvent<DataSetSelectedEvent>().Publish(testData);
            //navigationService.Navigate<DataSetsViewModel>();
            var typeFactory = this.GetTypeFactory();
            var v = typeFactory.CreateInstance<DataSetsView>();
            var vm = typeFactory.CreateInstance<DataSetsViewModel>();

            v.DataContext = vm;

            dockingService.AddAnchorable(vm,v);

            //uIVisualizerService.ShowAsync<bool>(DataSetsViewModel);

            //var employeeViewModel = new DataSetsViewModel();
            ////var uiVisualizerService = GetService<IUIVisualizerService>();
            //uIVisualizerService.Activate(employeeViewModel, "MainRegion");

            ////navigationService.Navigate<DataSetsViewModel>();
        }
    }
}
