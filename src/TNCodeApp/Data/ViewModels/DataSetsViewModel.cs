

using Prism.Mvvm;
using System.Collections.ObjectModel;
using TNCode.Core.Data;

namespace TNCodeApp.Data.ViewModels
{
    public class DataSetsViewModel : BindableBase
    {
        private ObservableCollection<IDataSet> dataSets;
        private IDataSet selectedDataSet;

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
                RaisePropertyChanged("ActiveDataSet");
            }
        }

        public DataSetsViewModel()
        {
            dataSets = new ObservableCollection<IDataSet>();
        }
    }
}
