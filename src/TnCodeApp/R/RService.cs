using Prism.Events;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TnCode.Core.Data;
using TnCode.Core.R;

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
        //private IEventAggregator eventAggregator;
        private IRManager rManager;

        public event EventHandler RConnected;
        public event EventHandler RDisconnected;

        public string WindowsDirectory
        {
            get;
            set;
        }

        public RService(ILoggerFacade loggerFacd, IRManager rMgr)
        {
            loggerFacade = loggerFacd;
            //eventAggregator = eventAgg;
            rManager = rMgr;
        }

        public bool IsRRunning { get => rManager.IsHostRunning(); }

        public Task DataSetToRAsDataFrameAsync(DataSet data)
        {
            throw new NotImplementedException();
        }

        public Task GenerateGgplotAsync(string plotCommand)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InitialiseAsync()
        {
            try
            {
                loggerFacade.Log("Connecting to R...", Category.Info, Priority.None);

                //var rHostSession = RHostSession.Create("TNCode");
                //rHostSession.Connected += RHostSession_Connected;
                //rHostSession.Disconnected += RHostSession_Disconnected;

                await rManager.StartHostAsync();
                await rManager.ExecuteAsync("library(" + string.Format("\"{0}\"", "R.devices") + ")");
                await rManager.ExecuteAsync("library(" + string.Format("\"{0}\"", "ggplot2") + ")");

                await rManager.ExecuteAndOutputAsync("setwd(" + ConvertPathToR(WindowsDirectory) + ")");
                loggerFacade.Log("Connected to R", Category.Info, Priority.None);

            }
            catch (Exception ex)
            {
                loggerFacade.Log("Failed to connect to R: " + ex.Message, Category.Exception, Priority.High);
                return false;
            }

            return true;
        }

        public Task LoadRWorkSpace(IProgress<string> arg1, string arg2)
        {
            throw new NotImplementedException();
        }

        public Task<string> RHomeFromConnectedRAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> RPlatformFromConnectedRAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> RVersionFromConnectedRAsync()
        {
            throw new NotImplementedException();
        }

        public string ConvertPathToR(string path)
        {
            string temp = path.Replace('\\', '/');
            return string.Format("\"{0}\"", temp);
        }

        Task<bool> IRService.GenerateGgplotAsync(string ggplotCommand)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRService.DataSetToRAsDataFrameAsync(DataSet data)
        {
            throw new NotImplementedException();
        }

        public Task LoadToTempEnv(string fullFileName)
        {
            throw new NotImplementedException();
        }

        public Task<List<object>> TempEnvObjects()
        {
            throw new NotImplementedException();
        }

        public Task RemoveTempEnviroment()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsDataFrame(string name)
        {
            throw new NotImplementedException();
        }

        public Task<DataSet> GetDataFrameAsDataSetAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<object>> ListWorkspaceItems()
        {
            throw new NotImplementedException();
        }
    }
}
