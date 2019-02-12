using ModuleR.Charts.Ggplot.Enums;
using ModuleR.Charts.Ggplot.Layer;
using ModuleR.Controls;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TNCode.Core.Data;

namespace ModuleR.ViewModels
{
    public class LayerViewModel: BindableBase, INavigationAware
    {
        private ILayer currentLayer;
        private List<string> variables;
        public List<string> Geoms { get; }
        private ObservableCollection<GgplotVariableControl> variableControls;
        private string selectedGeom;
        private IXmlConverter xmlConverter;

        public ObservableCollection<GgplotVariableControl> VariableControls
        {
            get { return variableControls; }
            set
            {
                variableControls = value;
                RaisePropertyChanged("ExtraControls");
            }
        }

        public List<string> Variables
        {
            get
            {
                return variables;
            }
            set
            {
                variables = value;
                RaisePropertyChanged("Variables");
            }
        }

        public string SelectedGeom
        {
            get { return selectedGeom; }
            set
            {
                selectedGeom = value;
                RaisePropertyChanged(value);
            }
        }

        public ILayer CurrentLayer
        {
            get => currentLayer;
            set
            {
                currentLayer = value;
                RaisePropertyChanged("CurrentLayer");
            }
        }

        public LayerViewModel(IXmlConverter converter)
        {
            xmlConverter = converter;

            Geoms = Enum.GetNames(typeof(Geoms)).ToList();
            Geoms.Remove("tile");

            variableControls = new ObservableCollection<GgplotVariableControl>();

            
        }

        private void UpdateGeomControls()
        {
            var aestheticXml = Properties.Resources.ResourceManager.GetObject("geom_" + selectedGeom.ToLower());
            var aesthetic = xmlConverter.ToObject<Aesthetic>(aestheticXml.ToString());
            CurrentLayer.Aes = aesthetic;
            variableControls.Clear();
            foreach (var aValue in aesthetic.AestheticValues)
            {
                var gControl = new GgplotVariableControl(aValue, variables);
                variableControls.Add(gControl);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //var layer = navigationContext.Parameters["layer"] as ILayer;
            //if (layer != null)
            //    return CurrentLayer != null && CurrentLayer.LastName == layer.LastName;
            //else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var layer = navigationContext.Parameters["layer"] as ILayer;
            if (layer != null)
                CurrentLayer = layer;

            variables = navigationContext.Parameters["variables"] as List<string>;

            SelectedGeom = CurrentLayer.Geom;

            UpdateGeomControls();
        }
    }
}
