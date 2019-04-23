using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Threading.Tasks;
using TNCodeApp.Docking;
using Unity;

namespace TNCodeApp.Progress
{
    public class ProgressViewModel : BindableBase, ITnPanel,IProgressService, INavigationAware
    {
        public IProgress<int> Progress { get; set; }

        private int progressValue;
        private bool progressIndeterminate;
        private string message = "Ready";

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

        public ProgressViewModel(IUnityContainer container)
        {
            container.RegisterInstance<IProgressService>(this);
        }

        public string Title { get => "Status"; }

        public DockingMethod Docking { get => DockingMethod.StatusPanel; }

        public async Task ExecuteAsync(IProgress<int> action, string message)
        {
            Progress = action;

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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
