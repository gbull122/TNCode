using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using TnCode.TnCodeApp.Charts.Events;
using TnCode.TnCodeApp.Charts.Views;
using TnCode.TnCodeApp.Data;
using TnCode.TnCodeApp.Data.Events;

namespace TnCode.TnCodeApp.Charts.ViewModels
{
    public class ChartRibbonViewModel : BindableBase
    {
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
        private IChartService chartService;
        private IDataSetsManager dataSetsManager;

        private bool variablesSelected;

        private IList<object> variables;

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

        public ChartRibbonViewModel(IEventAggregator eventAggr, IRegionManager regionMgr, IChartService chartMgr, IDataSetsManager dataMgr)
        {
            eventAggregator = eventAggr;
            regionManager = regionMgr;
            chartService = chartMgr;
            dataSetsManager = dataMgr;

            ChartCommand = new DelegateCommand<string>(CreateChart,CanCreatChart);

            eventAggregator.GetEvent<VariableSelectionChangedEvent>().Subscribe(VariableSelectionChanged);
            eventAggregator.GetEvent<ChartCreatedEvent>().Subscribe(ChartCreated);
        }

        private void ChartCreated(string obj)
        {
            regionManager.AddToRegion("MainRegion", new ChartView());
        }

        private bool CanCreatChart(string chartType)
        {
            switch (chartType)
            {
                case "scatter":
                    return true;
                case "line":
                    return true;
                default:
                    return false;
            }
        }

        private void VariableSelectionChanged()
        {
            ChartCommand.RaiseCanExecuteChanged();
        }

        private void CreateChart(string chartType)
        {
            var variables = dataSetsManager.SelectedVariables();
            chartService.Create(chartType, variables);
        }
    }
}
