using ModuleR.Charts.Ggplot.Enums;
using ModuleR.Charts.Ggplot.Layer;
using ModuleR.Controls;
using ModuleR.R;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TNCode.Core.Data;
using TNCodeApp.Data.Events;
using TNCodeApp.Docking;

namespace ModuleR.ViewModels
{
    public class ChartBuilderViewModel : BindableBase, ITnPanel, INavigationAware
    {
        private readonly IRManager rManager;
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
        private List<string> currentVariables;
        private string currentData;
        private IXmlConverter xmlConverter;

        public List<string> Geoms { get; }
        public DelegateCommand NewLayerCommand { get; private set; }
        public DelegateCommand ClearLayersCommand { get; private set; }
        public DelegateCommand<ILayer> LayerSelectedCommand { get; private set; }

        private ObservableCollection<VariableControl> variableControls;

        public ObservableCollection<VariableControl> VariableControls
        {
            get { return variableControls; }
            set
            {
                variableControls = value;
                RaisePropertyChanged("ExtraControls");
            }
        }

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
            }
        }

        public string SelectedGeom
        {
            get { return SelectedLayer == null ? "point" : SelectedLayer.Geom; }
            set
            {
                SelectedLayer.Geom = value;
                RaisePropertyChanged("SelectedGeom");
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

        public ChartBuilderViewModel(IEventAggregator eventAggr,IRegionManager regMngr,IRManager rMngr, IXmlConverter converter)
        {
            xmlConverter = converter;
            rManager = rMngr;
            eventAggregator = eventAggr;
            regionManager = regMngr;
            
            eventAggregator.GetEvent<DataSetSelectedEvent>().Subscribe(DataSetSelected, ThreadOption.UIThread);

            layers = new ObservableCollection<ILayer>();
            variableControls = new ObservableCollection<VariableControl>();

            Geoms = Enum.GetNames(typeof(Geoms)).ToList();
            Geoms.Remove("tile");

            NewLayerCommand = new DelegateCommand(NewLayer,CanNewLayer);
            ClearLayersCommand = new DelegateCommand(ClearLayers, CanClearLayers);
            LayerSelectedCommand = new DelegateCommand<ILayer>(LayerSelected);
            currentVariables = new List<string>();
            currentData = string.Empty;
        }

        private void LayerSelected(ILayer layer)
        {
            SelectedGeom = layer.Geom;
            var aestheticXml = Properties.Resources.ResourceManager.GetObject("geom_" + layer.Geom);
            var aesthetic = xmlConverter.ToObject<Aesthetic>(aestheticXml.ToString());
            MergeAesthetics(aesthetic);
            variableControls.Clear();
            foreach (var aValue in aesthetic.AestheticValues)
            {
                var gControl = new VariableControl(aValue, currentVariables);
                variableControls.Add(gControl);
            }
            RaisePropertyChanged("VariableControls");
        }

        private void MergeAesthetics(Aesthetic aestheticFromFile)
        {
            if (SelectedLayer.Aes == null)
                return;

            foreach (var aesValue in aestheticFromFile.AestheticValues)
            {
                if(!selectedLayer.Aes.DoesAestheticContainValue(aesValue.Name))
                {
                    selectedLayer.Aes.RemoveAestheticValue(aesValue.Name);
                }
            }
            RaisePropertyChanged("");
        }

        private bool CanClearLayers()
        {
            return layers.Count > 0;
        }

        private void ClearLayers()
        {
            Layers.Clear();
        }

        private bool CanNewLayer()
        {
            if ((currentVariables != null || currentVariables.Count > 0)&& !string.IsNullOrEmpty(currentData))
                return true;

            return false;
        }

        private void NewLayer()
        {
            var newLayer = new Layer("point");

            newLayer.Data = currentData;

            layers.Add(newLayer);

            SelectedLayer = newLayer;

            ClearLayersCommand.RaiseCanExecuteChanged();
        }

        private void DataSetSelected(DataSet dataSet)
        {
            currentVariables = dataSet.VariableNames();
            currentData = dataSet.Name;

            NewLayerCommand.RaiseCanExecuteChanged();
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
