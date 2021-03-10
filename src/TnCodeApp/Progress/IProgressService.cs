using System;
using System.Threading.Tasks;
using TnCode.TnCodeApp.Data;

namespace TnCode.TnCodeApp.Progress
{
    public interface IProgressService
    {
        string Message { get; set; }
        bool ProgressIndeterminate { get; set; }
        int ProgressValue { get; set; }
        Task ExecuteAsync(Func<IProgress<string>, string, Task> task, string arg);
        Task ExecuteAsync(Func<IProgress<string>, string, string, Task> task, string arg1, string arg2);
        Task ExecuteAsync(Func<IProgress<string>, IDataSet, Task> task, IDataSet arg);
        Task ExecuteAsync(Task<bool> task, string v);
        Task ContinueAsync(Task task, string v);
    }
}
