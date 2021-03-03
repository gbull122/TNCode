using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TnCode.Core.Data;

namespace TnCode.TnCodeApp.Data
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

            //foreach (var dataSet in dataSets)
            //{
            //    var selectedVariables = dataSet.SelectedVariableNames();
            //    if (selectedVariables.Count > 0)
            //        selection.Add(dataSet.Name, selectedVariables);
            //}

            return selection;
        }

        public List<string> SelectedDataSetsNames()
        {
            throw new NotImplementedException();
            //var dataSetNames = new List<string>();
            //var selectedData = SelectedData();
            //fora

            //return selection;
        }

        public IList<IVariable> SelectedVariables()
        {
            throw new NotImplementedException();
            //var selection = new List<IVariable>();
            //foreach (var dataSet in dataSets)
            //{
            //    foreach (var vari in dataSet.Variables)
            //    {
            //        if (vari.IsSelected)
            //            selection.Add(vari);
            //    }
            //}

            //return selection;
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
                if (dataSet.Name.Equals(dataSetName))
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

        public List<string[]> ReadCsvFileRowWise(string filePath)
        {
            List<string[]> rawDataRowWise = new List<string[]>();

            using (var reader = new StreamReader(File.OpenRead(filePath)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    rawDataRowWise.Add(line.Split(','));
                }
            }

            return rawDataRowWise;
        }

        public List<string[]> RowWiseToColumnWise(List<string[]> rowWiseData)
        {
            var numberOfColumns = rowWiseData[0].Length;
            var numberOfRows = rowWiseData.Count;
            List<string[]> colWiseData = new List<string[]>(numberOfColumns);


            for (int index = 0; index < numberOfColumns; ++index)
            {
                var col = new List<string>();
                foreach (var row in rowWiseData)
                {
                    col.Add(row[index]);
                }

                colWiseData.Add(col.ToArray());
            }

            return colWiseData;
        }

        public bool DataSetAdd(IDataSet dataSet)
        {
            if (dataSets.Contains(dataSet))
                return false;

            DataSets.Add(new SelectableDataSet(dataSet));
            return true;
        }

        public IDataSet DataSetGet(string name)
        {
            foreach (var dataSet in dataSets)
            {
                if (dataSet.Name.Equals(name))
                    return dataSet;
            }
            return null;
        }

    }
}