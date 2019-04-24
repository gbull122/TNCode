using System.Collections.ObjectModel;
using TNCode.Core.Data;

namespace TNCodeApp.Data
{
    public class DataSetsManager : IDataSetsManager
    {

        private ObservableCollection<IDataSet> dataSets;

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
