using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TNCode.Core.Data;
using TNCodeApp.Data.Events;
using TNCodeApp.Docking;
using TNCodeApp.R;

namespace TNCodeApp.Data.ViewModels
{
    public class DataSetsViewModel : BindableBase, ITnPanel, INavigationAware, IRegionMemberLifetime
    {
        private IDataSet selectedDataSet;
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
        private IList<object> selectedVariables;
        private IDataSetsManager datasetsManager;
        private IRManager rManager;

        public string Title { get => "Data"; }

        public DockingMethod Docking { get => DockingMethod.ControlPanel; }

        public DelegateCommand VariableSelectionChangedCommand { get; private set; }
        public DelegateCommand DatasetSelectionChangedCommand { get; private set; }
        public DelegateCommand DeleteDataSetCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }

        public ObservableCollection<IDataSet> DataSets
        {
            get { return datasetsManager.DataSets; }
            set
            {
                datasetsManager.DataSets = value;
                RaisePropertyChanged("DataSets");
            }
        }

        public IDataSet SelectedDataSet
        {
            get { return selectedDataSet; }
            set
            {
                selectedDataSet = value;
                RaisePropertyChanged("SelectedDataSet");
            }
        }

        public IList<object> SelectedVariables
        {
            get { return selectedVariables; }
            set
            {
                selectedVariables = value;
                RaisePropertyChanged("SelectedVariable");
            }
        }

        public bool KeepAlive => true;

        public DataSetsViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IDataSetsManager dataMgr,IRManager rMgr)
        {
            datasetsManager = dataMgr;
            rManager = rMgr;

            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;

            DatasetSelectionChangedCommand = new DelegateCommand(DatasetSelectionChanged);
            DeleteDataSetCommand = new DelegateCommand(DeleteDataSet);
            VariableSelectionChangedCommand = new DelegateCommand(VariableSelectionChanged);
            CloseCommand = new DelegateCommand(Close);

            eventAggregator.GetEvent<TestDataEvent>().Subscribe(TestData, ThreadOption.UIThread);
            eventAggregator.GetEvent<NewDataSetEvent>().Subscribe(AddNewDataset, ThreadOption.UIThread);

        }

        private void Close()
        {
            throw new NotImplementedException();
        }

        private void VariableSelectionChanged()
        {
            eventAggregator.GetEvent<VariablesSelectedChangedEvent>().Publish();
        }

        private void DeleteDataSet()
        {
            DataSets.Remove(selectedDataSet);
        }

        private async void AddNewDataset(DataSet dataSet)
        {
            await rManager.DataSetToRAsDataFrameAsync(dataSet);
            datasetsManager.DataSets.Add(dataSet);
        }

        private void DatasetSelectionChanged()
        {
            eventAggregator.GetEvent<DataSetsSelectedChangedEvent>().Publish();
        }

        private void TestData(string obj)
        {
            if (datasetsManager.DatasetNameExists("Mpg"))
                return;

            var mpgData = new MpgDataSet();

            var dataList = new List<object[,]>() { mpgData.Data };

            var testData = new DataSet(dataList, "Mpg");

            eventAggregator.GetEvent<NewDataSetEvent>().Publish(testData);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedDataSet = datasetsManager.DataSets[0];
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
