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
using TNCodeApp.Data.Events;

namespace TNCodeApp.Chart.ViewModels
{
    public class ChartRibbonViewModel:BindableBase
    {
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
        private bool variablesSelected;
        private IList<object> variables;
        private IChartManager chartManager;

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

        public ChartRibbonViewModel(IEventAggregator eventAggr, IRegionManager regionMgr,IChartManager chartMgr)
        {
            eventAggregator = eventAggr;
            regionManager = regionMgr;
            chartManager = chartMgr;

            ChartCommand = new DelegateCommand<string>(CreateChart).ObservesCanExecute(() => VariablesSelected);

            eventAggregator.GetEvent<VariablesSelectedEvent>().Subscribe(VariablesSelection);
            eventAggregator.GetEvent<ChartCreatedEvent>().Subscribe(ChartCreated);
        }

        private void ChartCreated(string obj)
        {
            regionManager.AddToRegion("MainRegion", new ChartView());
        }

        private void VariablesSelection(IList<object> variableList)
        {
            if (variableList.Count > 0)
            {
                variables = variableList;
                VariablesSelected = true;
            }
        }

        private void CreateChart(string chartType)
        {
            chartManager.Create(chartType, variables);
            
        }
    }
}
