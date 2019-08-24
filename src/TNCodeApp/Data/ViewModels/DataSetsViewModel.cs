using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TNCode.Core.Data;
using TNCodeApp.Docking;

namespace TNCodeApp.Data.ViewModels
{
    public class DataSetsViewModel : ViewModelBase, ITnPanel
    {
        private IDataSet selectedDataSet;
        //private IEventAggregator eventAggregator;
        //private IRegionManager regionManager;
        private IList<object> selectedVariables;
        private IDataSetsManager datasetsManager;

        public string Title { get => "Data Sets"; }

        public DockingMethod Docking { get => DockingMethod.ControlPanel; }

        //public DelegateCommand<IList<object>> VariableSelectionChangedCommand { get; private set; }
        //public DelegateCommand SelectedItemChangedCommand { get; private set; }
        //public DelegateCommand DeleteDataSetCommand { get; private set; }
        //public DelegateCommand CloseCommand { get; private set; }

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

        public DataSetsViewModel( IDataSetsManager dataMgr)
        {
            datasetsManager = dataMgr;

            //this.eventAggregator = eventAggregator;
            //this.regionManager = regionManager;

            //SelectedItemChangedCommand = new DelegateCommand(SelectedItemChanged);
            //DeleteDataSetCommand = new DelegateCommand(DeleteDataSet);
            //VariableSelectionChangedCommand = new DelegateCommand<IList<object>>(VariableSelectionChanged);
            //CloseCommand = new DelegateCommand(Close);

            //eventAggregator.GetEvent<TestDataEvent>().Subscribe(TestData, ThreadOption.UIThread);
            //eventAggregator.GetEvent<DataLoadedEvent>().Subscribe(LoadData, ThreadOption.UIThread);

        }

        private void Close()
        {
            throw new NotImplementedException();
        }

        private void VariableSelectionChanged(IList<object> variableList)
        {
            SelectedVariables = variableList;
            //eventAggregator.GetEvent<VariablesSelectedEvent>().Publish(variableList);
        }

        private void DeleteDataSet()
        {
            DataSets.Remove(selectedDataSet);
        }

        private void LoadData(DataSet obj)
        {
            datasetsManager.DataSets.Add(obj);

            //eventAggregator.GetEvent<DataSetSelectedEvent>().Publish(obj);
        }

        private void SelectedItemChanged()
        {
            //eventAggregator.GetEvent<DataSetSelectedEvent>().Publish(obj);
        }

        private void TestData(string obj)
        {
            var mpgData = new MpgDataSet();

            var dataList = new List<object[,]>() { mpgData.Data };

            var testData = new DataSet(dataList, "Mpg");
            datasetsManager.DataSets.Add(testData);

            //eventAggregator.GetEvent<DataSetSelectedEvent>().Publish(testData);
        }

       
    }
}
