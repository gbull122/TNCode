using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using TnCode.TnCodeApp.Data.Events;

namespace TnCode.TnCodeApp.Charts.ViewModels
{
    public class ChartRibbonViewModel : BindableBase
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

            ChartCommand = new DelegateCommand<string>(CreateChart,CanCreatChart);

            eventAggregator.GetEvent<VariablesSelectedEvent>().Subscribe(VariablesSelection);
        }

        private bool CanCreatChart(string chartType)
        {
            switch (chartType)
            {
                case "Scatter":
                    return true;
                case "Line":
                    return false;
                default:
                    return false;
            }
        }

        private void VariablesSelection(Dictionary<string, ICollection<string>> variableList)
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
