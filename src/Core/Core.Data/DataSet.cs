using System;
using System.Collections.Generic;
using System.Linq;

namespace TNCode.Core.Data
{
    public class DataSet : IDataSet
    {
        private List<IVariable> variables = new List<IVariable>();
        private List<string> variableNames = new List<string>();
        private List<string> observationNames = new List<string>();
        private int rowCount;

        public string Name { get; }

        public List<string> VariableNames => variableNames;

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
                    variableNames.Add(dColumn.Name);
                }
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
    }
}
