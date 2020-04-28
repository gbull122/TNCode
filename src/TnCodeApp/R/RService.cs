using Prism.Events;
using Prism.Logging;
using System;
using System.Threading.Tasks;
using TnCode.Core.Data;

namespace TnCode.TnCodeApp.R
{
    public interface IRService
    {
        Action<object, EventArgs> RDisconnected { get; set; }
        Action<object, EventArgs> RConnected { get; set; }
        bool IsRRunning { get; set; }

        Task LoadRWorkSpace(IProgress<string> arg1, string arg2);
        Task DataSetToRAsDataFrameAsync(DataSet data);
        Task GenerateGgplotAsync(string plotCommand);
        Task<string> RHomeFromConnectedRAsync();
        Task<string> RVersionFromConnectedRAsync();
        Task<string> RPlatformFromConnectedRAsync();
        Func<IProgress<string>, string, Task> InitialiseAsync();
    }

    public class RService: IRService
    {
        private ILoggerFacade loggerFacade;
        private IEventAggregator eventAggregator;

        public RService(ILoggerFacade loggerFacade, IEventAggregator eventAggregator)
        {
            this.loggerFacade = loggerFacade;
            this.eventAggregator = eventAggregator;
        }

        public Action<object, EventArgs> RDisconnected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<object, EventArgs> RConnected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsRRunning { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task DataSetToRAsDataFrameAsync(DataSet data)
        {
            throw new NotImplementedException();
        }

        public Task GenerateGgplotAsync(string plotCommand)
        {
            throw new NotImplementedException();
        }

        public Func<IProgress<string>, string, Task> InitialiseAsync()
        {
            throw new NotImplementedException();
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
    }
}
