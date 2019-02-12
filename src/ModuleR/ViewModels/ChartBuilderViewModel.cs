using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ModuleR.Charts.Ggplot.Layer;
using ModuleR.R;
using ModuleR.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using TNCode.Core.Data;
using TNCodeApp.Data.Events;
using TNCodeApp.Docking;

namespace ModuleR.ViewModels
{
    public class ChartBuilderViewModel : BindableBase, ITnPanel, INavigationAware
    {
        private IRManager rManager;
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
        private List<string> currentVariables;
        private string currentData;

        public DelegateCommand NewLayerCommand { get; private set; }

        public string Title => "Plot";

        public DockingMethod Docking => DockingMethod.Document;

        private ILayer selectedLayer;

        public ILayer SelectedLayer
        {
            get => selectedLayer;
            set
            {
                selectedLayer = value;
                RaisePropertyChanged("SelectedLayer");
                UpdateLayer();
            }
        }

        private ObservableCollection<ILayer> layers;

        public ObservableCollection<ILayer> Layers
        {
            get { return layers; }
            set
            {
                layers = value;
                RaisePropertyChanged("ChartLayers");
            }
        }

        public ChartBuilderViewModel(IEventAggregator eventAggr,IRegionManager regMngr,IRManager rMngr)
        {
            rManager = rMngr;
            eventAggregator = eventAggr;
            regionManager = regMngr;
            
            eventAggregator.GetEvent<DataSetSelectedEvent>().Subscribe(DataSetSelected, ThreadOption.UIThread);

            layers = new ObservableCollection<ILayer>();

            NewLayerCommand = new DelegateCommand(NewLayer);
        }

        private void UpdateLayer()
        {
            var parameters = new NavigationParameters();
            parameters.Add("layer", selectedLayer);

            regionManager.RequestNavigate("LayerRegion", "LayerView", parameters);
        }

        private void NewLayer()
        {
            var newLayer = new Layer("point");

            SelectedLayer = newLayer;
            newLayer.Data = currentData;

            layers.Add(newLayer);

            var parameters = new NavigationParameters();
            parameters.Add("layer", newLayer);
            parameters.Add("variables", currentVariables);

            regionManager.RequestNavigate("LayerRegion", "LayerView",parameters);
        }

        private void DataSetSelected(DataSet dataSet)
        {
            currentVariables = dataSet.VariableNames();
            currentData = dataSet.Name;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext == null)
               return;

            NewLayer();
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
