using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TNCode.Core.Data;

namespace TNCodeApp.Data
{
    public interface IDataSetsManager
    {
        ObservableCollection<IDataSet> DataSets { get; set; }
        Dictionary<string, ICollection<string>> SelectedData();
        IList<IVariable> SelectedVariables();
        List<string> SelectedDataSetsNames();
        IEnumerable<string> DataSetNames();
        List<string> DataSetVariableNames(string dataSetName);
        bool DatasetNameExists(string name);
        List<string[]> ReadCsvFile(string filePath);
    }
}