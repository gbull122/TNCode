using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Threading.Tasks;
using Unity;

namespace TnCode.TnCodeApp.Progress
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

        public async Task ExecuteAsync(Func<IProgress<string>, string, Task> task, string arg1)
        {
            var progress = new Progress<string>(taskMessage =>
            {
                Message = taskMessage;
            });

            ProgressIndeterminate = true;

            await Task.Run(() => task(progress, arg1));

            ProgressIndeterminate = false;
            Message = "Ready";
        }

        public async Task ExecuteAsync(Func<IProgress<string>, string, string, Task> task, string arg1, string arg2)
        {
            var progress = new Progress<string>(taskMessage =>
            {
                Message = taskMessage;
            });

            ProgressIndeterminate = true;

            await Task.Run(() => task(progress, arg1, arg2));

            ProgressIndeterminate = false;
            Message = "Ready";
        }

        //public async Task ExecuteAsync(Func<IProgress<int>> action, string message)
        //{
        //    //var progress = new Progress<int>(actionProgress =>
        //    //{
        //    //    ProgressValue = actionProgress;
        //    //});

        //    //await Task.Run(() => action(progress));
        //}

        public async Task ExecuteAsync(Task<bool> task, string v)
        {
            Message = v;
            ProgressIndeterminate = true;

            await Task.Run(() => task);

            ProgressIndeterminate = false;
            Message = "Ready";
        }

        //public async Task ExecuteAsync(Task task, string v)
        //{
        //    Message = v;
        //    ProgressIndeterminate = true;

        //    task.Start();
        //    await task;

        //    ProgressIndeterminate = false;
        //    Message = "Ready";
        //}

        public async Task ContinueAsync(Task task, string v)
        {
            Message = v;
            ProgressIndeterminate = true;

            await task;

            ProgressIndeterminate = false;
            Message = "Ready";

        }

        //public async Task<T> ContinueAsync<T>(Task<T> task, string v)
        //{
        //    Message = v;
        //    ProgressIndeterminate = true;

        //    var result  = await task;

        //    ProgressIndeterminate = false;
        //    Message = "Ready";

        //    return result;
        //}

    }

}
