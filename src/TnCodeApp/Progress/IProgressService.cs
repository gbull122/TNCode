using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TnCode.TnCodeApp.Progress
{
    public interface IProgressService
    {
        string Message { get; set; }
        bool ProgressIndeterminate { get; set; }
        int ProgressValue { get; set; }
        Task ExecuteAsync(Func<IProgress<string>, string, Task> task, string arg);
        Task ExecuteAsync(Func<IProgress<string>, string, string, Task> task, string arg1, string arg2);
        //Task ExecuteAsync(IProgress<int> action, string message);
        Task ExecuteAsync(Task<bool> task, string v);
        //Task ExecuteAsync(Task task, string v);
        Task ContinueAsync(Task task, string v);
        //Task<T> ContinueAsync<T>(Task<T> task, string v);

    }
}
