using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        public Dictionary<string, ICollection<string>> SelectedData()
        {
            var selection = new Dictionary<string, ICollection<string>>();

            foreach (var dataSet in dataSets)
            {
                var selectedVariables = dataSet.SelectedVariableNames();
                if (selectedVariables.Count > 0)
                    selection.Add(dataSet.Name, selectedVariables);
            }

            return selection;
        }

        public List<string> SelectedDataSetsNames()
        {
            var selection = new List<string>();
            foreach (var dataSet in dataSets)
            {
                var selectedVariables = dataSet.SelectedVariableNames();
                if (selectedVariables.Count > 0)
                    selection.Add(dataSet.Name);
            }

            return selection;
        }

        public IList<IVariable> SelectedVariables()
        {
            var selection = new List<IVariable>();
            foreach (var dataSet in dataSets)
            {
                foreach (var vari in dataSet.Variables)
                {
                    if (vari.IsSelected)
                        selection.Add(vari);
                }
            }

            return selection;
        }

        public IEnumerable<string> DataSetNames()
        {
            foreach (var dataSet in dataSets)
            {
                yield return dataSet.Name;
            }
        }

        public List<string> DataSetVariableNames(string dataSetName)
        {
            foreach (var dataSet in dataSets)
            {
                if(dataSet.Name.Equals(dataSetName))
                    return dataSet.VariableNames();
            }
            return null;
        }

        public bool DatasetNameExists(string name)
        {
            foreach (var dataSet in dataSets)
            {
                if (dataSet.Name.Equals(name))
                    return true;
            }
            return false;
        }

        public List<string[]> ReadCsvFile(string filePath)
        {
            var reader = new StreamReader(File.OpenRead(filePath));
            List<string[]> rawData = new List<string[]>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                rawData.Add(line.Split(','));
            }

            return rawData;
        }



    }
}
