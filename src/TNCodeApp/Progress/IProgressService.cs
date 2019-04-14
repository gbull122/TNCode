using System;
using System.Threading.Tasks;

namespace TNCodeApp.Progress
{
    public interface IProgressService
    {
        string Message { get; set; }
        bool ProgressIndeterminate { get; set; }
        int ProgressValue { get; set; }
        Task ExecuteAsync(IProgress<int> action, string message);
        Task ExecuteAsync(Task<bool> task, string v);
    }
}
