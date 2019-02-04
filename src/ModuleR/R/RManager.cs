using Microsoft.R.Host.Client;
using Prism.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNCode.Core.Data;

namespace ModuleR.R
{
    public class RManager : IRManager
    {
        private IROperations rOperations;
        private readonly IRHostSessionCallback rHostSessionCallback;
        private ILoggerFacade logger;

        public bool IsRRunning
        {
            get
            {
                return rOperations != null && rOperations.IsHostRunning();
            }
        }

        public RManager(IRHostSessionCallback rhostSession, ILoggerFacade loggerFacade)
        {
            logger = loggerFacade;
            rHostSessionCallback = rhostSession;
        }

        public RManager(IRHostSessionCallback rhostSession, ILoggerFacade loggerFacade, IROperations rOps)
        {
            logger = loggerFacade;
            rHostSessionCallback = rhostSession;
            rOperations = rOps;
        }

        public async Task<bool> InitialiseAsync()
        {
            try
            {
                var rHostSession = RHostSession.Create("TNCode");
                rOperations = new ROperations(rHostSession);

                await rOperations.StartHostAsync(rHostSessionCallback);
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message, Category.Exception, Priority.High);
                return false;
            }
            return true;
        }

        public async Task DeleteVariablesAsync(List<string> names)
        {
            foreach (string name in names)
            {
                await rOperations.ExecuteAsync("rm(" + name + ")");
            }
        }

        public async Task<DataFrame> GetDataFrameAsync(string name)
        {
            DataFrame result = null;
            try
            {
                result = await rOperations.GetDataFrameAsync(name);
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message, Category.Exception, Priority.High);
            }
            return result;
        }

        public async Task<DataSet> GetDataFrameAsDataSetAsync(string name)
        {
            var dataFrame = await GetDataFrameAsync(name);

            var dataSet = ConvertDataFrameToDataSet(dataFrame, name);

            return dataSet;
        }

        public DataSet ConvertDataFrameToDataSet(DataFrame data, string name)
        {
            DataSet result = null;

            result = new DataSet(data.Data, data.ColumnNames, name);

            return result;
        }

        //public async Task<DataSet> ConvertListToDataSet(RSessionOutput data)
        //{

        //}

        public async Task<object[,]> GetRListAsync(string name)
        {
            object[,] result = null;
            var listNames = await rOperations.ExecuteAndOutputAsync("names(" + name + ")");
            var names = listNames.Output.Replace("\"", "");
            var thing = names.Split(' ');
            thing = thing.Skip(1).ToArray();
            List<List<object>> data = new List<List<object>>();
            for (int i = 0; i < thing.Count(); i++)
            {
                var rlist = await rOperations.GetListAsync(name + "$" + thing[i]);
                data.Add(rlist);
            }
            result = ListOfVectorsToObject(data, thing);
            return result;
        }

        public object[,] ListOfVectorsToObject(List<List<object>> data, string[] names)
        {
            var maxLength = MaximumLengthOfList(data);

            object[,] result;
            int startRow = 0;
            if (names != null)
            {
                result = new object[maxLength + 1, data.Count];
                startRow = 1;
                for (int s = 0; s < names.Length; s++)
                {
                    result[0, s] = names[s];
                }
            }
            else
            {
                result = new object[maxLength, data.Count];
            }


            for (int column = 0; column < data.Count; column++)
            {
                for (int row = startRow; row < data[column].Count + startRow; row++)
                {
                    result[row, column] = data[column][row - startRow];
                }
            }
            return result;
        }

        public int MaximumLengthOfList(List<List<object>> data)
        {
            int maxLength = 0;
            foreach (var myVector in data)
            {
                if (myVector.Count > maxLength)
                {
                    maxLength = myVector.Count;
                }
            }
            return maxLength;
        }

        public async Task<string> RunRCommnadAsync(string code)
        {
            if (!rOperations.IsHostRunning())
                return string.Empty;

            return await rOperations.EvaluateAsync<string>(code);
        }

        public async Task<string> RHomeFromConnectedRAsync()
        {
            return await RunRCommnadAsync("R.home()");
        }

        public async Task<string> RPlatformFromConnectedRAsync()
        {
            return await RunRCommnadAsync("version$platform");
        }

        public async Task<string> RMinorVersionFromConnectedRAsync()
        {
            return await RunRCommnadAsync("R.version$minor");
        }

        public async Task<string> RVersionFromConnectedRAsync()
        {
            return await RunRCommnadAsync("R.version$version.string");
        }

        public async Task<bool> DataSetToRAsDataFrameAsync(DataSet data,List<IReadOnlyCollection<object>> selectedData, string name, string[] headers)
        {
            //var rowLength = selectedData[0].Cast<object>().ToList().Count;
            //var rows = Enumerable.Range(1, rowLength);

            //List<string> rowNames = ((IEnumerable)rows)
            //                    .Cast<object>()
            //                    .Select(x => x.ToString())
            //                    .ToList();


            //DataFrame df = new DataFrame(data.ObservationNames.AsReadOnly(), data.VariableNames().AsReadOnly(), data.);

            //await rOperations.CreateDataFrameAsync(name, df);

            return true;
        }
    }
}
