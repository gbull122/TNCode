using Microsoft.R.Host.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TnCode.Core.R
{
    public interface IRManager
    {
        IRHostSession HostSession { get; }
        bool IsHostRunning();
        Task StartHostAsync();
        Task ExecuteCommandAsync(string command);
        Task<DataFrame> GetDataFrameAsync(string name);
        Task<RSessionOutput> ExecuteAndOutputAsync(string command);
        Task<List<object>> GetListAsync(string command);
        Task<string> EvaluateAsync<T>(string command);
        Task ExecuteAsync(string command);
        Task CreateDataFrameAsync(string name, DataFrame dataFrame);
    }
}
