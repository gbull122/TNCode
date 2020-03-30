using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Threading.Tasks;
using TNCodeApp.Docking;
using Unity;

namespace TNCodeApp.Progress
{
    public class ProgressViewModel : BindableBase, IProgressService, IRegionMemberLifetime//, INavigationAware
    {
        public IProgress<int> Progress { get; set; }

        private int progressValue;
        private bool progressIndeterminate;
        private string message = "Ready";
        private IEventAggregator eventAggregator;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                RaisePropertyChanged("Message");
            }
        }

        public bool ProgressIndeterminate
        {
            get { return progressIndeterminate; }
            set
            {
                progressIndeterminate = value;
                RaisePropertyChanged("ProgressIndeterminate");
            }
        }

        public int ProgressValue
        {
            get { return progressValue; }
            set
            {
                progressValue = value;
                RaisePropertyChanged("ProgressValue");
            }
        }

        public ProgressViewModel(IUnityContainer container, IEventAggregator eventAgg)
        {
            container.RegisterInstance<IProgressService>(this);
            eventAggregator = eventAgg;

            eventAggregator.GetEvent<DisplayMessageEvent>().Subscribe(ShowMessage);
        }

        private void ShowMessage(string message)
        {
            Message = message;
        }

        public bool KeepAlive => true;

        public async Task ExecuteAsync(Func<IProgress<string>,string,Task> task,string arg)
        {
            var progress = new Progress<string>(taskMessage =>
            {
                Message = taskMessage;
            });

            await Task.Run(() => task(progress,arg));
        }

        public void ReportProgress(string pregress)
        {

        }

        public async Task ExecuteAsync(IProgress<int> action, string message)
        {
            
            await Task.Run(() => action);
            
        }

        public async Task ExecuteAsync(Task<bool> task, string v)
        {
            Message = v;
            ProgressIndeterminate = true;

            await Task.Run(() => task);

            ProgressIndeterminate = false;
            Message = "Ready";
        }

        //public void OnNavigatedTo(NavigationContext navigationContext)
        //{
            
        //}

        //public bool IsNavigationTarget(NavigationContext navigationContext)
        //{
        //    return true;
        //}

        //public void OnNavigatedFrom(NavigationContext navigationContext)
        //{
            
        //}

        public async Task ExecuteAsync(Task task, string v)
        {
            Message = v;
            ProgressIndeterminate = true;

            task.Start();
            await task;

            ProgressIndeterminate = false;
            Message = "Ready";
        }

        public async Task ContinueAsync(Task task, string v)
        {
            Message = v;
            ProgressIndeterminate = true;

            await task;

            ProgressIndeterminate = false;
            Message = "Ready";

        }

        public async Task<T> ContinueAsync<T>(Task<T> task, string v)
        {
            Message = v;
            ProgressIndeterminate = true;

            var result  = await task;

            ProgressIndeterminate = false;
            Message = "Ready";

            return result;
        }
    }
}
