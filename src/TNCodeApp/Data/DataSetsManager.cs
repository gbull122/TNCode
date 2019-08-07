using System.Collections.ObjectModel;
using System.ComponentModel;
using TNCode.Core.Data;

namespace TNCodeApp.Data
{
    public class DataSetsManager : IDataSetsManager, INotifyPropertyChanged
    {

        private ObservableCollection<IDataSet> dataSets;
        private IDataSet selectedDataSet;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public IDataSet SelectedDataSet
        {
            get { return selectedDataSet; }
            set { selectedDataSet = value; }
        }

        public ObservableCollection<IDataSet> DataSets
        {
            get { return dataSets; }
            set
            {
                dataSets = value;
            }
        }

        public DataSetsManager()
        {
            dataSets = new ObservableCollection<IDataSet>();
        }
    }
}
