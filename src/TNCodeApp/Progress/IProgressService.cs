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

        Task ExecuteAsync(Task task, string v);
        Task ContinueAsync(Task task, string v);

        Task<T> ContinueAsync<T>(Task<T> task, string v);

        Task ExecuteAsync(Func<IProgress<string>, string, Task> task, string arg);
    }
}
