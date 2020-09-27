using Microsoft.R.Host.Client;
using Prism.Events;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TnCode.Core.Data;
using TnCode.Core.R;
using TnCode.TnCodeApp.Data.Events;

namespace TnCode.TnCodeApp.R
{
    public interface IRService
    {
        Task<bool> InitialiseAsync();
        bool IsRRunning { get; }
        event EventHandler RConnected;
        event EventHandler RDisconnected;
        Task<bool> GenerateGgplotAsync(string ggplotCommand);
        Task<bool> DataSetToRAsDataFrameAsync(DataSet data);
        Task<string> RHomeFromConnectedRAsync();
        Task<string> RPlatformFromConnectedRAsync();
        Task<string> RVersionFromConnectedRAsync();
        Task LoadToTempEnv(string fullFileName);
        Task<List<object>> TempEnvObjects();
        Task RemoveTempEnviroment();
        Task<bool> IsDataFrame(string name);
        Task<DataSet> GetDataFrameAsDataSetAsync(string name);
        Task<List<object>> ListWorkspaceItems();
        Task LoadRWorkSpace(IProgress<string> progress, string fileName);
    }

    public class RService: IRService
    {
        private ILoggerFacade loggerFacade;
        private IRManager rManager;
        private IEventAggregator eventAggregator;

        public event EventHandler RConnected;
        public event EventHandler RDisconnected;

        public string WindowsDirectory
        {
            get;
            set;
        }

        public RService(ILoggerFacade loggerFacd, IRManager rMgr ,string path, IEventAggregator eventAgg)
        {
            loggerFacade = loggerFacd;
            rManager = rMgr;
            WindowsDirectory = path;
            eventAggregator = eventAgg;
        }

        public bool IsRRunning { get => rManager.IsHostRunning(); }

        public async Task<bool> DataSetToRAsDataFrameAsync(DataSet data)
        {
            DataFrame df = new DataFrame(data.ObservationNames.AsReadOnly(), data.VariableNames(), data.RawData());

            await rManager.CreateDataFrameAsync(data.Name, df);

            return true;
        }

        //public async Task<bool> GenerateGgplotAsync(string plotCommand)
        //{
        //    try
        //    {
        //        using (StringReader reader = new StringReader(plotCommand))
        //        {
        //            var plotWidth = 15;
        //            var plotHeight = 12;
        //            var plotRes = 600;
        //            //get code into text file
        //            string fileName = WindowsDirectory + "\\TNGgplot.R";
        //            using (StreamWriter file = new StreamWriter(fileName))
        //            {
        //                file.WriteLine(
        //                    "devEval(" + string.Format("\"{0}\"", "png") +
        //                    ", path = " + ConvertPathToR(WindowsDirectory) +
        //                    ", name = \"TNGgplot\", width = " + plotWidth +
        //                    ", height = " + plotHeight + ", units =" +
        //                    string.Format("\"{0}\"", "cm") + ", res = " + plotRes + ", pointsize = 12, {");

        //                string line;
        //                while ((line = reader.ReadLine()) != null)
        //                {
        //                    file.WriteLine(line);
        //                }
        //                file.WriteLine("plot(p)");
        //                file.WriteLine("})");
        //            }

        //            await rManager.ExecuteAsync("source(" + ConvertPathToR(fileName) + ",echo=TRUE, max.deparse.length=10000)");
        //        }
        //    }
        //    catch
        //    {
        //        //ErrorMessage = "Failed to generate plot " + ex.Message;
        //        return false;
        //    }
        //    return true;
        //}

        public async Task<bool> InitialiseAsync()
        {
            try
            {
                loggerFacade.Log("Connecting to R...", Category.Info, Priority.None);

                var rHostSession = rManager.HostSession;
                rHostSession.Connected += RHostSession_Connected;
                rHostSession.Disconnected += RHostSession_Disconnected;

                await rManager.StartHostAsync();
                
                await rManager.ExecuteAsync("library(" + string.Format("\"{0}\"", "R.devices") + ")");
                loggerFacade.Log("Library R.devices loaded", Category.Info, Priority.None);
                await rManager.ExecuteAsync("library(" + string.Format("\"{0}\"", "ggplot2") + ")");
                loggerFacade.Log("Library ggplot2 loaded", Category.Info, Priority.None);

                await rManager.ExecuteAndOutputAsync("setwd(" + ConvertPathToR(WindowsDirectory) + ")");
                

            }
            catch (Exception ex)
            {
                loggerFacade.Log("Failed to connect to R: " + ex.Message, Category.Exception, Priority.High);
                return false;
            }
            loggerFacade.Log("Connected to R", Category.Info, Priority.None);
            return true;
        }

        private void RHostSession_Disconnected(object sender, EventArgs e)
        {
            RDisconnected?.Invoke(this, e);
        }

        private void RHostSession_Connected(object sender, EventArgs e)
        {
            RConnected?.Invoke(this, e);
        }

        public async Task<string> RunRCommnadAsync(string code)
        {
            return await rManager.EvaluateAsync<string>(code);
        }

        public async Task LoadRWorkSpace(IProgress<string> progress, string fileName)
        {
            progress.Report("Loading workspace");

            await LoadToTempEnv(fileName);

            var tempItems = await TempEnvObjects();
            var workspaceItems = await ListWorkspaceItems();

            foreach (string tempItem in tempItems)
            {
                ///TODO give option to overwrite
                if (!workspaceItems.Contains(tempItem))
                {
                    var isDataFrame = await IsDataFrame(tempItem);
                    if (isDataFrame)
                    {
                        progress.Report("Loading dataframe " + tempItem);
                        var importedData = await GetDataFrameAsDataSetAsync(tempItem);

                        var dataSetEventArgs = new DataSetEventArgs();
                        dataSetEventArgs.Modification = DataSetChange.AddedFromR;
                        dataSetEventArgs.Data = importedData;

                        eventAggregator.GetEvent<DataSetChangedEvent>().Publish(dataSetEventArgs);
                    }
                }
            }

            await RemoveTempEnviroment();
        }

        public async Task<string> RHomeFromConnectedRAsync()
        {
            return await RunRCommnadAsync("R.home()");
        }

        public async Task<string> RPlatformFromConnectedRAsync()
        {
            return await RunRCommnadAsync("version$platform");
        }

        public async Task<string> RVersionFromConnectedRAsync()
        {
            return await RunRCommnadAsync("R.version$version.string");
        }

        public string ConvertPathToR(string path)
        {
            string temp = path.Replace('\\', '/');
            return string.Format("\"{0}\"", temp);
        }

        async Task<bool> IRService.GenerateGgplotAsync(string ggplotCommand)
        {
            try
            {
                using (StringReader reader = new StringReader(ggplotCommand))
                {
                    var plotWidth = 15;
                    var plotHeight = 12;
                    var plotRes = 600;
                    //get code into text file
                    string fileName = WindowsDirectory + "\\TNGgplot.R";
                    using (StreamWriter file = new StreamWriter(fileName))
                    {
                        file.WriteLine(
                            "devEval(" + string.Format("\"{0}\"", "png") +
                            ", path = " + ConvertPathToR(WindowsDirectory) +
                            ", name = \"TNGgplot\", width = " + plotWidth +
                            ", height = " + plotHeight + ", units =" +
                            string.Format("\"{0}\"", "cm") + ", res = " + plotRes + ", pointsize = 12, {");

                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            file.WriteLine(line);
                        }
                        file.WriteLine("plot(p)");
                        file.WriteLine("})");
                    }

                    await rManager.ExecuteAsync("source(" + ConvertPathToR(fileName) + ",echo=TRUE, max.deparse.length=10000)");
                }
            }
            catch(Exception ex)
            {
                var errorMessage = "Failed to generate plot " + ex.Message;
                loggerFacade.Log(errorMessage, Category.Exception, Priority.High);
                
                return false;
            }
            return true;
        }

        public async Task LoadToTempEnv(string fullFileName)
        {
            var rPath = ConvertPathToR(fullFileName);

            await rManager.ExecuteAsync("tempEnv<-new.env()");
            await rManager.ExecuteAsync("load(" + rPath + ",envir=tempEnv)");
        }

        public async Task<List<object>> TempEnvObjects()
        {
            await rManager.ExecuteAsync("tempVars<-ls(tempEnv)");
            return await rManager.GetListAsync("tempVars");
        }

        public async Task RemoveTempEnviroment()
        {
            await rManager.ExecuteAsync("rm(tempEnv)");
        }

        public async Task<bool> IsDataFrame(string name)
        {
            var classProps = await rManager.GetListAsync("class(tempEnv$" + name + ")");
            foreach (string prop in classProps)
            {
                if (prop.Equals("data.frame"))
                {
                    await rManager.ExecuteAsync(name + "<-tempEnv$" + name);
                    await rManager.ExecuteAsync("rm(" + name + ",envir=tempEnv)");
                    return true;
                }

            }
            return false;
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


        public async Task<DataFrame> GetDataFrameAsync(string name)
        {
            DataFrame result = null;
            try
            {
                result = await rManager.GetDataFrameAsync(name);
            }
            catch (Exception ex)
            {
                loggerFacade.Log(ex.Message, Category.Exception, Priority.High);
            }
            return result;
        }

        public async Task<List<object>> ListWorkspaceItems()
        {
            await rManager.ExecuteAsync("vars<-ls()");
            return await rManager.GetListAsync("vars");
        }
    }
}
