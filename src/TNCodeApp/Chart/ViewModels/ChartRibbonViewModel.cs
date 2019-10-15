using System;
using System.Collections.Generic;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using TNCode.Core.Data;
using TNCodeApp.Chart.Events;
using TNCodeApp.Chart.Views;
using TNCodeApp.Data;
using TNCodeApp.Data.Events;

namespace TNCodeApp.Chart.ViewModels
{
    public class ChartRibbonViewModel:BindableBase
    {
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
        private IChartManager chartManager;
        private IDataSetsManager dataSetsManager;

        private bool variablesSelected;
        private IList<string> variableNames;
        

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

        public ChartRibbonViewModel(IEventAggregator eventAggr, IRegionManager regionMgr,IChartManager chartMgr, IDataSetsManager dataMgr)
        {
            eventAggregator = eventAggr;
            regionManager = regionMgr;
            chartManager = chartMgr;
            dataSetsManager = dataMgr;

            ChartCommand = new DelegateCommand<string>(CreateChart).ObservesCanExecute(() => VariablesSelected);

            eventAggregator.GetEvent<VariablesSelectedEvent>().Subscribe(VariablesSelection);
            eventAggregator.GetEvent<ChartCreatedEvent>().Subscribe(ChartCreated);
        }

        private void ChartCreated(string obj)
        {
            regionManager.AddToRegion("MainRegion", new ChartView());
        }

        private void VariablesSelection(Dictionary<string,ICollection<string>> variableList)
        {
            if (variableList.Count > 0)
            { 
                VariablesSelected = true;
            }
        }

        private void CreateChart(string chartType)
        {
            var variables = dataSetsManager.SelectedVariables();
            chartManager.Create(chartType, variables);
        }
    }
}
