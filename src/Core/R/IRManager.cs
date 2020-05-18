using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TnCode.Core.Data;

namespace TnCode.Core.R
{
    public interface IRManager
    {
        bool InitialiseAsync();
        bool IsRRunning { get; }

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
}
