using System;
using System.Collections.ObjectModel;
using ModuleR.Charts.Ggplot.Layer;
using ModuleR.R;
using Prism.Commands;
using Prism.Events;
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
                RaisePropertyChanged();
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

        public ChartBuilderViewModel(IEventAggregator eventAggr, IRManager rMngr)
        {
            rManager = rMngr;
            eventAggregator = eventAggr;

            eventAggregator.GetEvent<DataSetSelectedEvent>().Subscribe(DataSetSelected, ThreadOption.UIThread);

            layers = new ObservableCollection<ILayer>();
            var aLayer = new Layer();
            layers.Add(aLayer);

            NewLayerCommand = new DelegateCommand(NewLayer);
        }

        
        private void NewLayer()
        {
            var newLayer = new Layer();

            layers.Add(newLayer);
        }

        private void DataSetSelected(DataSet obj)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext == null)
               return;
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
