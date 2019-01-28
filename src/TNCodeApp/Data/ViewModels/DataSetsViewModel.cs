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

namespace TNCodeApp.Data.ViewModels
{
    public class DataSetsViewModel : BindableBase, ITnPanel, INavigationAware
    {
        private ObservableCollection<IDataSet> dataSets;
        private IDataSet selectedDataSet;
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
        private IVariable selectedVariable;

        public string Title { get => "Data Sets"; }

        public DockingMethod Docking { get => DockingMethod.Left; }

        public DelegateCommand<DataSet> SelectedItemChangedCommand { get; private set; }

        public ObservableCollection<IDataSet> DataSets
        {
            get { return dataSets; }
            set
            {
                dataSets = value;
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

        public IVariable SelectedVariable
        {
            get { return selectedVariable; }
            set
            {
                selectedVariable = value;
                RaisePropertyChanged("SelectedVariable");
            }
        }

        public DataSetsViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            dataSets = new ObservableCollection<IDataSet>();

            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;

            SelectedItemChangedCommand = new DelegateCommand<DataSet>(SelectedItemChanged);

            eventAggregator.GetEvent<TestDataEvent>().Subscribe(TestData, ThreadOption.UIThread);
            eventAggregator.GetEvent<DataLoadedEvent>().Subscribe(LoadData, ThreadOption.UIThread);


        }

        private void LoadData(DataSet obj)
        {
            dataSets.Add(obj);

            eventAggregator.GetEvent<DataSetSelectedEvent>().Publish(obj);
        }

        private void SelectedItemChanged(DataSet obj)
        {
            eventAggregator.GetEvent<DataSetSelectedEvent>().Publish(obj);
        }

        private void TestData(string obj)
        {
            var mpgData = new MpgDataSet();

            var dataList = new List<object[,]>() { mpgData.Data };

            var testData = new DataSet(dataList, "Mpg");
            dataSets.Add(testData);

            eventAggregator.GetEvent<DataSetSelectedEvent>().Publish(testData);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new System.NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
