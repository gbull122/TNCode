using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Data;
using TNCodeApp.Docking;

namespace TNCodeApp.Data.ViewModels
{
    public class DataViewModel : BindableBase, ITnPanel, INavigationAware
    {
        public string Title => displayedSet.DataSetName;
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
