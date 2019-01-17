using Prism.Mvvm;
using Prism.Regions;
using System;
using TNCode.Core.Data;
using TNCodeApp.Docking;
using System.Linq;


namespace TNCodeApp.Data.ViewModels
{
    public class DataViewModel : BindableBase, ITnPanel, INavigationAware
    {
        public string title;

        public string Title
        {
           get => title;
            set => title=value;
        }

        public DockingMethod Docking { get => DockingMethod.Document; }

        public DataSet displayedSet;

        public DataSet DisplayedSet
        {
            get { return displayedSet; }
            set
            {
                displayedSet = value;
                RaisePropertyChanged();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext == null)
                return;

            var datasetName = navigationContext.Parameters.Keys.ElementAt(0);

            Title = datasetName;
            DisplayedSet = navigationContext.Parameters[datasetName] as DataSet;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext == null)
                return false;

            var name = navigationContext.Parameters["Name"] as string;
            if (!string.IsNullOrEmpty(name))
            {
                return string.Compare(name, Title, StringComparison.OrdinalIgnoreCase) == 0;
            }
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public DataViewModel()
        {

        }


    }
}
