using Microsoft.Extensions.Logging;
using Microsoft.R.Host.Client;
using OxyPlot;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using TnCode.Core.R;
using TnCode.Core.R.Charts.Ggplot;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.TnCodeApp.Data;
using TnCode.TnCodeApp.Data.Events;

namespace TnCode.TnCodeApp.R
{
    public interface IRService
    {
        Task<bool> InitialiseAsync(IProgress<string> progress);
        bool IsRRunning { get; }
        event EventHandler RConnected;
        event EventHandler RDisconnected;
        Task<bool> GenerateGgplotAsync(string ggplotCommand);
        Task DataSetToRAsDataFrameAsync(IProgress<string> progress, IDataSet data);
        Task<string> RHomeFromConnectedRAsync();
        Task<string> RPlatformFromConnectedRAsync();
        Task<string> RVersionFromConnectedRAsync();
        Task LoadToTempEnv(string fullFileName);
        Task<List<object>> TempEnvObjects();
        Task RemoveTempEnviroment();
        Task<bool> IsDataFrame(string name);
        Task<IDataSet> GetDataFrameAsDataSetAsync(string name);
        Task<List<object>> ListWorkspaceItems();
        Task LoadRWorkSpace(IProgress<string> progress, string fileName);
        IGgplot GetGgplot(ObservableCollection<ILayer> layers);
    }

    public class RService: IRService
    {
        private ILogger logger;
        private IRManager rManager;
        private IEventAggregator eventAggregator;

        public event EventHandler RConnected;
        public event EventHandler RDisconnected;

        public string WindowsDirectory
        {
            get;
            set;
        }

        public RService(ILogger logger, IRManager rMgr ,string path, IEventAggregator eventAgg)
        {
            this.logger = logger;
            rManager = rMgr;
            WindowsDirectory = path;
            eventAggregator = eventAgg;
        }

        public bool IsRRunning { get => rManager.IsHostRunning(); }

        public async Task DataSetToRAsDataFrameAsync(IProgress<string> progress,IDataSet data)
        {
            progress.Report(string.Format("Copying DataSet {0} to R",data.Name));

            DataFrame df = new DataFrame(data.ObservationNames.AsReadOnly(), data.VariableNames(), data.RawData());

            await rManager.CreateDataFrameAsync(data.Name, df);
        }


        public async Task Initilaise(Func<IProgress<string>, Task> task)
        {
            var progress = new Progress<string>(taskMessage =>
            {
                logger.LogInformation(taskMessage);
            });


            await Task.Run(() => task(progress));

        }

        public async Task<bool> InitialiseAsync(IProgress<string> progress)
        {
            try
            {
                progress.Report("Connecting to R...");

                var rHostSession = rManager.HostSession;
                rHostSession.Connected += RHostSession_Connected;
                rHostSession.Disconnected += RHostSession_Disconnected;

                await rManager.StartHostAsync();

                string rdevices = string.Format("\"{0}\"", "R.devices");
                var test = $"library {rdevices}";
                await rManager.ExecuteAsync($"library({rdevices})");
                progress.Report("Library R.devices loaded");
                string ggplot2 = string.Format("\"{0}\"", "ggplot2");
                await rManager.ExecuteAsync($"library({ggplot2})");
                progress.Report("Library ggplot2 loaded");

                await rManager.ExecuteAndOutputAsync("setwd(" + ConvertPathToR(WindowsDirectory) + ")");
            }
            catch (Exception ex)
            {
                progress.Report("Failed to connect to R: " + ex.Message + ex.StackTrace);
                return false;
            }
            progress.Report("Connected to R");
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
                logger.LogInformation(errorMessage);
                
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

        public async Task<IDataSet> GetDataFrameAsDataSetAsync(string name)
        {
            var dataFrame = await GetDataFrameAsync(name);

            var dataSet = ConvertDataFrameToDataSet(dataFrame, name);

            return dataSet;
        }

        public IDataSet ConvertDataFrameToDataSet(DataFrame data, string name)
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
                logger.LogInformation(ex.Message);
            }
            return result;
        }

        public async Task<List<object>> ListWorkspaceItems()
        {
            await rManager.ExecuteAsync("vars<-ls()");
            return await rManager.GetListAsync("vars");
        }

        public IGgplot GetGgplot(ObservableCollection<ILayer> layers)
        {
            Ggplot plot = new Ggplot();
            foreach(ILayer layer in layers)
            {
                plot.AddLayer(layer);
            }
            
            return plot;
        }
    }
}
