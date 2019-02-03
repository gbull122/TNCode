using System.Collections.Generic;
using System.Linq;

namespace TNCode.Core.Data
{
    public class DataSet : IDataSet
    {
        private List<IVariable> variables = new List<IVariable>();
        private List<string> observationNames = new List<string>();
        private int rowCount;

        public string Name { get; }

        public List<string> ObservationNames => observationNames;

        public List<IVariable> Variables => variables;

        public DataSet(List<object[,]> rawData, string name)
        {
            this.Name = name;
            foreach (object[,] area in rawData)
            {
                rowCount = area.GetLength(0);
                int columnCount = area.GetLength(1);

                int firstColumnIndex = area.GetLowerBound(1);

                for (int col = firstColumnIndex; col < columnCount + firstColumnIndex; col++)
                {
                    var dColumn = new Variable(GetAColumn(area, col));
                    variables.Add(dColumn);
                }
            }
        }

        public DataSet(List<string[]> rawData, string name)
        {
            Name = name;

            foreach(var col in rawData)
            {
                var dColumn = new Variable(col);
                variables.Add(dColumn);
            }
        }

        public DataSet(IReadOnlyList<IReadOnlyList<object>> data, IReadOnlyList<string> columnNames,string name)
        {
            Name = name;

            for(int variable=0;variable<data.Count;variable++)
            {
                var thing = new List<object>(data[variable]);
                thing.Insert(0, columnNames[variable]);

                var dColumn = new Variable(thing.ToArray());
                variables.Add(dColumn);
            }
        }

        public object[] GetAColumn(object[,] rawData, int col)
        {
            object[] result = new object[rowCount];

            int firstRowIndex = rawData.GetLowerBound(0);

            for (int row = firstRowIndex; row < rowCount + firstRowIndex; row++)
            {
                if (rawData[row, col] != null)
                {
                    result[row - firstRowIndex] = rawData[row, col];
                }
                else
                {
                    result[row - firstRowIndex] = "NaN";
                }
            }
            return result;
        }

        public bool AllColumnsEqual()
        {
            var length = Variables[0].Length;
            var test = from dc in Variables
                       where dc.Length == length
                       select dc;

            return test.Count() == Variables.Count;
        }

        public List<string> VariableNames()
        {
            var names = new List<string>();
            {
                foreach(var variable in variables)
                {
                    names.Add(variable.Name);
                }
            }
            return names;
        }


    }
}
