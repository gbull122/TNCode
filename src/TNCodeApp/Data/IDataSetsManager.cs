using System.Collections.Generic;
using System.Collections.ObjectModel;
using TNCode.Core.Data;

namespace TNCodeApp.Data
{
    public interface IDataSetsManager
    {
        bool DataSetAdd(IDataSet dataSet);
        IDataSet DataSetGet(string name);
        Dictionary<string, ICollection<string>> SelectedData();
        IList<IVariable> SelectedVariables();
        List<string> SelectedDataSetsNames();
        IEnumerable<string> DataSetNames();
        List<string> DataSetVariableNames(string dataSetName);
        bool DatasetNameExists(string name);
        List<string[]> ReadCsvFileRowWise(string filePath);
        List<string[]> RowWiseToColumnWise(List<string[]> rowWiseData);
    }
}