using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TNCode.Core.Data;

namespace TNCodeApp.R
{
    public interface IRManager
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
    }
}