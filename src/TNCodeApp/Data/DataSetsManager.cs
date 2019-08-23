using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TNCode.Core.Data;

namespace TNCodeApp.Data
{
    public class DataSetsManager : IDataSetsManager, INotifyPropertyChanged
    {

        private ObservableCollection<IDataSet> dataSets;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public Dictionary<string, List<string>> SelectedData()
        {
            var selection = new Dictionary<string, List<string>>();

            foreach (var dataSet in dataSets)
            {
                var selectedVariables = dataSet.SelectedVariableNames();
                if (dataSet.IsSelected || selectedVariables.Count > 0)
                    selection.Add(dataSet.Name, selectedVariables);
            }

            return selection;
        }
    }
}
