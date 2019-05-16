using System;
using System.Collections.Generic;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using TNCodeApp.Chart.Views;
using TNCodeApp.Data.Events;

namespace TNCodeApp.Chart.ViewModels
{
    public class ChartRibbonViewModel:BindableBase
    {
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
        private bool variablesSelected;

        public DelegateCommand<string> ChartCommand { get; private set; }

        public bool VariablesSelected
        {
            get => variablesSelected;
            set
            {
                variablesSelected = value;
                RaisePropertyChanged("VariablesSelected");
            }
        }

        public ChartRibbonViewModel(IEventAggregator eventAggr, IRegionManager regionMgr)
        {
            eventAggregator = eventAggr;
            regionManager = regionMgr;

            ChartCommand = new DelegateCommand<string>(CreateChart).ObservesCanExecute(() => VariablesSelected);

            eventAggregator.GetEvent<VariablesSelectedEvent>().Subscribe(VariablesSelection);

            //
        }

        private void VariablesSelection(IList<object> variableList)
        {
            if (variableList.Count > 0)
                VariablesSelected = true;
        }

        private void CreateChart(string chartType)
        {
            var navigationParameters = new NavigationParameters();

            regionManager.RequestNavigate("MainRegion",
                new Uri("ChartView" + navigationParameters.ToString(), UriKind.Relative));
        }
    }
}
