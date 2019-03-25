using ModuleR.Charts.Ggplot;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
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
        private BitmapImage chartImage;

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

        public BitmapImage ChartImage
        {
            get { return chartImage; }
            set
            {
                chartImage = value;
                RaisePropertyChanged("ChartImage");
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

        public ChartBuilderViewModel(IEventAggregator eventAggr, IRegionManager regMngr, IRManager rMngr, IXmlConverter converter)
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

            NewLayerCommand = new DelegateCommand(NewLayer, CanNewLayer);
            ClearLayersCommand = new DelegateCommand(ClearLayers, CanClearLayers);
            LayerSelectedCommand = new DelegateCommand<ILayer>(LayerSelected);
            currentVariables = new List<string>();
            currentData = string.Empty;

            this.PropertyChanged += ChartBuilderViewModel_PropertyChanged;
        }

        private void ChartBuilderViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (e.PropertyName.Equals("SelectedGeom"))
            //{
            //    Update();
            //    GeneratePlotAsync();
            //}

        }

        private void LayerSelected(ILayer layer)
        {
            SelectedLayer.PropertyChanged-= SelectedLayer_PropertyChanged;
            SelectedLayer = layer;
            SelectedLayer.PropertyChanged += SelectedLayer_PropertyChanged;
            foreach (var vc in variableControls)
            {
                vc.PropertyChanged -= GControl_PropertyChanged;
            }
            variableControls.Clear();
            foreach (var aValue in SelectedLayer.Aes.AestheticValues)
            {
                var gControl = new VariableControl(aValue, currentVariables);
                gControl.PropertyChanged += GControl_PropertyChanged;
                variableControls.Add(gControl);
            }
            
            RaisePropertyChanged("VariableControls");
        }

        private void SelectedLayer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Geom"))
                Update();
        }

        private void Update()
        {
            var aesthetic = LoadAesthetic(SelectedLayer.Geom);
            MergeAesthetics(aesthetic);

            foreach (var vc in variableControls)
            {
                vc.PropertyChanged -= GControl_PropertyChanged;
            }
            variableControls.Clear();
            foreach (var aValue in SelectedLayer.Aes.AestheticValues)
            {
                var gControl = new VariableControl(aValue, currentVariables);
                gControl.PropertyChanged += GControl_PropertyChanged;
                variableControls.Add(gControl);
            }
            RaisePropertyChanged("VariableControls");
        }

        private Aesthetic LoadAesthetic(string geom)
        {
            var aestheticXml = Properties.Resources.ResourceManager.GetObject("geom_" + geom);
            var aesthetic = xmlConverter.ToObject<Aesthetic>(aestheticXml.ToString());
            return aesthetic;
        }

        private async void GControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await GeneratePlotAsync();
        }

        private async Task GeneratePlotAsync()
        {
            var plot = new Ggplot();
            plot.Layers.AddRange(layers);
            var plotCommand = plot.Command();

            await rManager.GenerateGgplotAsync(plotCommand);

            var imagePath = GetGgplotChartPath();

            if (!string.IsNullOrEmpty(imagePath))
            {
                using (var stream = new FileStream(
                            imagePath,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.Read))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    ChartImage = bitmap;
                }
            }
        }

        public string GetGgplotChartPath()
        {
            DirectoryInfo directoryToSearch = new DirectoryInfo(Path.GetTempPath());
            FileInfo[] filesInDir = directoryToSearch.GetFiles("*.png");
            foreach (FileInfo foundFile in filesInDir)
            {
                if (foundFile.FullName.EndsWith("TNGgplot.png"))
                {
                    return foundFile.FullName;
                }
            }

            return string.Empty;
        }

        private void MergeAesthetics(Aesthetic aestheticFromFile)
        {
            if (SelectedLayer.Aes == null)
                return;

            var mergedAesthetic = new Aesthetic();

            mergedAesthetic.DefaultStat = aestheticFromFile.DefaultStat;
            mergedAesthetic.DefaultPosition = aestheticFromFile.DefaultPosition;

            foreach (var aesValue in aestheticFromFile.AestheticValues)
            {
                if (SelectedLayer.Aes.DoesAestheticContainValue(aesValue.Name))
                {
                    mergedAesthetic.AestheticValues.Add(SelectedLayer.Aes.GetAestheticValueByName(aesValue.Name));

                }
                else
                {
                    mergedAesthetic.AestheticValues.Add(aesValue);
                }
            }
            SelectedLayer.Aes = mergedAesthetic;

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
            if ((currentVariables != null || currentVariables.Count > 0) && !string.IsNullOrEmpty(currentData))
                return true;

            return false;
        }

        private void NewLayer()
        {
            var newLayer = new Layer("point");

            var aestheticXml = Properties.Resources.ResourceManager.GetObject("geom_point");
            var aesthetic = xmlConverter.ToObject<Aesthetic>(aestheticXml.ToString());
            newLayer.Aes = aesthetic;
            newLayer.Data = currentData;

            layers.Add(newLayer);

            ClearLayersCommand.RaiseCanExecuteChanged();
            SelectedLayer = newLayer;
        }

        private async void PutDataSetInR(DataSet data)
        {
            await rManager.DataSetToRAsDataFrameAsync(data);
        }

        private void DataSetSelected(DataSet dataSet)
        {
            var varibleNames = dataSet.VariableNames();
            varibleNames.Insert(0, "");
            currentVariables = varibleNames;

            currentData = dataSet.Name;

            PutDataSetInR(dataSet);
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
