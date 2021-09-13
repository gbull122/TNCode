using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TnCode.TnCodeApp.Data.Events;
using TnCode.TnCodeApp.Docking;
using TnCode.TnCodeApp.R;

namespace TnCode.TnCodeApp.Data.ViewModels
{
    public class DataSetsViewModel : BindableBase, IDockingPanel, INavigationAware, IRegionMemberLifetime
    {
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
        private IDataSetsManager datasetsManager;
        private IRService rService;

        public string Title { get => "Data"; }

        public DockingMethod Docking { get => DockingMethod.ControlPanel; }

        public DelegateCommand<string> VariableSelectionChangedCommand { get; private set; }
        public DelegateCommand DatasetSelectionChangedCommand { get; private set; }

        public DelegateCommand<IDataSet> DeleteDataSetCommand { get; private set; }
        public DelegateCommand<IDataSet> CopyDataSetExcelCommand { get; private set; }

        public ObservableCollection<IDataSet> DataSets
        {
            get { return datasetsManager.DataSets; }
            set
            {
                datasetsManager.DataSets = value;
                RaisePropertyChanged(nameof(DataSets));
            }
        }

        public bool KeepAlive => true;

        public DataSetsViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IDataSetsManager dataMgr, IRService rSer)
        {
            datasetsManager = dataMgr;
            rService = rSer;

            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;

            DatasetSelectionChangedCommand = new DelegateCommand(DatasetSelectionChanged);
            DeleteDataSetCommand = new DelegateCommand<IDataSet>(DeleteDataSet);
            CopyDataSetExcelCommand = new DelegateCommand<IDataSet>(CopyDatSetExcel);
            VariableSelectionChangedCommand = new DelegateCommand<string>(VariableSelectionChanged);

            eventAggregator.GetEvent<DataSetChangedEvent>().Subscribe(AddNewDataset, ThreadOption.UIThread);

        }

        private void VariableSelectionChanged(string name)
        {
            eventAggregator.GetEvent<VariableSelectionChangedEvent>().Publish();
        }

        private void DeleteDataSet(IDataSet dataSet)
        {
            //DataSets.Remove(selectedDataSet);
        }

        private void CopyDatSetExcel(IDataSet obj)
        {
            throw new NotImplementedException();
        }

        private void AddNewDataset(DataSetEventArgs dataSetEventArgs)
        {
            if (dataSetEventArgs.Modification == DataSetChange.Added)
            {
                datasetsManager.DataSetAdd(dataSetEventArgs.Data);
                dataSetEventArgs.Modification = DataSetChange.Updated;
                eventAggregator.GetEvent<DataSetChangedEvent>().Publish(dataSetEventArgs);
            }
        }

        private void DatasetSelectionChanged()
        {
            //eventAggregator.GetEvent<DataSetsSelectedChangedEvent>().Publish();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //SelectedDataSet = datasetsManager.DataSets[0];
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

