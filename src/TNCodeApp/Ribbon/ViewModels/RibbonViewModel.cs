using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows;
using System.Windows.Threading;

namespace TNCodeApp.Menu.ViewModels
{
    public class RibbonViewModel : BindableBase, ITnRibbon
    {

        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;

        public DelegateCommand AboutCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand SettingsCommand { get; private set; }

        public InteractionRequest<INotification> CustomPopupRequest { get; set; }
        public DelegateCommand CustomPopupCommand { get; set; }

        public bool IsMainRibbon => true;

        public RibbonViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            CloseCommand = new DelegateCommand(Close);
            AboutCommand = new DelegateCommand(About);
            SettingsCommand = new DelegateCommand(Settings);

            CustomPopupRequest = new InteractionRequest<INotification>();
        }


        private void Settings()
        {

        }

        private void About()
        {
            CustomPopupRequest.Raise(new Notification
            {
                Title = "Custom Popup",
                Content = "Custom Popup Message "
            });

        }

        private void Close()
        {
            Application.Current.Shutdown();
        }

    }
}
