using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;

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

        public IEnumerable<IDataSet> SelectedDataSets()
        {
            var dataSetNames = new List<IDataSet>();

            foreach (var dataSet in DataSets)
            {
                if (dataSet.IsSelected)
                    dataSetNames.Add(dataSet);
            }

            return dataSetNames;
        }

        public IList<string> SelectedDataSetsNames()
        {
            var dataSetNames = new List<string>();

            foreach(var dataSet in DataSets)
            {
                if (dataSet.IsSelected)
                    dataSetNames.Add(dataSet.Name);
            }

            return dataSetNames;
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

        public IList<string> DataSetVariableNames(string dataSetName)
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

        public void SaveAllDataSetsToCsv(string path)
        {
            foreach (var dataSet in dataSets)
            {
                var destination = Path.Combine(path, dataSet.Name + ".csv");
                var rowWiseData = ColumnWiseToRowWise(dataSet.RawData());
                using (var writer = new StreamWriter(destination))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    object[] varNames = dataSet.VariableNames().ToArray<object>();
                    rowWiseData.Insert(0, varNames);

                    foreach (var row in rowWiseData)
                    {
                        foreach (var value in row)
                        {
                            csv.WriteField(value);
                            
                        }
                        csv.NextRecord();
                    }
                    writer.Flush();

                }
            }
        }

        //TODO use csv helper
        public IList<string[]> ReadCsvFileRowWise(string filePath)
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

        public List<string[]> RowWiseToColumnWise(IList<string[]> rowWiseData)
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

        public List<object[]> ColumnWiseToRowWise(IReadOnlyList<IReadOnlyList<object>> columnWiseData)
        {
            var numberOfColumns =  columnWiseData.Count;
            var numberOfRows = columnWiseData[0].Count;

            List<object[]> rowWiseData = new List<object[]>(numberOfColumns);


            for (int index = 0; index < numberOfRows; ++index)
            {
                var row = new List<object>();
                foreach (var col in columnWiseData)
                {
                    row.Add(col[index]);
                }

                rowWiseData.Add(row.ToArray());
            }

            return rowWiseData;
        }

        public bool DataSetAdd(IDataSet dataSet)
        {
            if (dataSets.Contains(dataSet))
                return false;

            DataSets.Add(dataSet);
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