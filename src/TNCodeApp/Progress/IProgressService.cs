using System;
using System.Threading.Tasks;

namespace TNCodeApp.Progress
{
    public interface IProgressService
    {
        Task ExecuteAsync(IProgress<int> action, string message);
    }
}
