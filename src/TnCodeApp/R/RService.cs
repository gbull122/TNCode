using Prism.Events;
using Prism.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TnCode.Core.Data;
using TnCode.Core.R;

namespace TnCode.TnCodeApp.R
{
    public interface IRService
    {
        bool IsRRunning { get; }
        Task LoadRWorkSpace(IProgress<string> arg1, string arg2);
        Task DataSetToRAsDataFrameAsync(DataSet data);
        Task GenerateGgplotAsync(string plotCommand);
        Task<string> RHomeFromConnectedRAsync();
        Task<string> RVersionFromConnectedRAsync();
        Task<string> RPlatformFromConnectedRAsync();
        bool InitialiseAsync();
    }

    public class RService: IRService
    {
        private ILoggerFacade loggerFacade;
        private IEventAggregator eventAggregator;
        private IRManager rManager;

        public RService(ILoggerFacade loggerFacd, IEventAggregator eventAgg, IRManager rMgr)
        {
            loggerFacade = loggerFacd;
            eventAggregator = eventAgg;
            rManager = rMgr;
        }

        public bool IsRRunning { get => rManager.IsRRunning; }

        public Task DataSetToRAsDataFrameAsync(DataSet data)
        {
            throw new NotImplementedException();
        }

        public Task GenerateGgplotAsync(string plotCommand)
        {
            throw new NotImplementedException();
        }

        public bool InitialiseAsync()
        {
            return rManager.InitialiseAsync();
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
