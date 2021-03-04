using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TnCode.TnCodeApp.Data
{
    public interface IDataSetsManager
    {
        ObservableCollection<IDataSet> DataSets { get; set; }
        IDataSet DataSetGet(string name);
        bool DataSetAdd(IDataSet dataSet);
        IEnumerable<IDataSet> SelectedDataSets();
        IList<IVariable> SelectedVariables();
        IList<string> SelectedDataSetsNames();
        IEnumerable<string> DataSetNames();
        IList<string> DataSetVariableNames(string dataSetName);
        bool DatasetNameExists(string name);
        IList<string[]> ReadCsvFileRowWise(string filePath);
        List<string[]> RowWiseToColumnWise(IList<string[]> rowWiseData);
    }
}
