using System;
using System.Threading;
using System.Threading.Tasks;

namespace TNCodeApp.Progress
{
    public class ProgressService : IProgressService
    {

        public ProgressService()
        {

        }

        public async Task ExecuteAsync(IProgress<int> action, string message)
        {
            await DoAsync(action);
        }

        private async Task DoAsync(IProgress<int> action)
        {

            await Task.Run(() => action);
        }

    }
}
