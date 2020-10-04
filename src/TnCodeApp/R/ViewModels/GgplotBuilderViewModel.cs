using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TnCode.Core.R.Charts.Ggplot;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.Core.Utilities;
using TnCode.TnCodeApp.Data;
using TnCode.TnCodeApp.Data.Events;
using TnCode.TnCodeApp.Docking;
using TnCode.TnCodeApp.Progress;
using TnCode.TnCodeApp.R.Controls;
using TnCode.TnCodeApp.R.Views;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class GgplotBuilderViewModel:BindableBase, IDockingPanel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IContainerExtension containerExtension;

        private readonly IRService rService;
        private readonly IDataSetsManager dataSetsManager;

        private readonly IProgressService progressService;

        private Ggplot ggplot;

        private List<string> currentVariables;
        private IEnumerable<string> dataSets;
        private readonly IXmlConverter xmlConverter;
        private BitmapImage chartImage;
        private List<Parameter> titleParameters;
        private StatViewModel statViewModel;

        public string Title => "Chart Builder";

        public DockingMethod Docking => DockingMethod.Document;

        public List<string> Geoms { get; }
        
        public List<string> Positions { get; }
        public DelegateCommand NewLayerCommand { get; private set; }
        public DelegateCommand ClearLayersCommand { get; private set; }
        public DelegateCommand<ILayer> LayerSelectedCommand { get; private set; }
       
        public DelegateCommand<string> SelectedGeomChangedCommand { get; private set; }
        public DelegateCommand<string> SelectedPositionChangedCommand { get; private set; }
        public DelegateCommand CopyChartCommand { get; private set; }
        public DelegateCommand<string> ActionCommand { get; private set; }

        private ObservableCollection<VariableControl> geomControls;
        private ObservableCollection<IOptionControl> statControls;
        private ObservableCollection<IOptionControl> positionControls;

        public List<string> Variables
        {
            get => currentVariables;
            set
            {
                currentVariables = value;
                RaisePropertyChanged(nameof(Variables));
            }
        }


        public IEnumerable<string> DataSets
        {
            get { return dataSets; }
            set
            {
                dataSets = value;
                RaisePropertyChanged(nameof(DataSets));
            }
        }

        public ObservableCollection<VariableControl> GeomControls
        {
            get { return geomControls; }
            set
            {
                geomControls = value;
                RaisePropertyChanged(nameof(GeomControls));
            }
        }

      

        public ObservableCollection<IOptionControl> PositionControls
        {
            get { return positionControls; }
            set
            {
                positionControls = value;
                RaisePropertyChanged(nameof(PositionControls));
            }
        }

        public BitmapImage ChartImage
        {
            get { return chartImage; }
            set
            {
                chartImage = value;
                RaisePropertyChanged(nameof(ChartImage));
            }
        }

        private string xVariableFacet;

        public string XVariableFacet
        {
            get => xVariableFacet;
            set
            {
                xVariableFacet = value;
                RaisePropertyChanged(nameof(XVariableFacet));
            }
        }

        private string yVariableFacet;

        public string YVariableFacet
        {
            get => yVariableFacet;
            set
            {
                yVariableFacet = value;
                RaisePropertyChanged(nameof(YVariableFacet));
            }
        }

        private ILayer selectedLayer;

        public ILayer SelectedLayer
        {
            get => selectedLayer;
            set
            {
                if (selectedLayer != null)
                    selectedLayer.PropertyChanged -= SelectedLayer_PropertyChanged;

                
                selectedLayer = value;

                selectedLayer.PropertyChanged += SelectedLayer_PropertyChanged;
                RaisePropertyChanged(nameof(SelectedLayer));
            }
        }

       

        private string selectedPosition;

        public string SelectedPosition
        {
            get => selectedPosition;
            set
            {
                selectedPosition = value;
                RaisePropertyChanged(nameof(SelectedPosition));
            }
        }

        public List<ILayer> Layers
        {
            get { return ggplot.Layers; }

        }

        private StatView statViewContent;

        public StatView StatViewContent
        {
            get { return statViewContent; }
        }


        public GgplotBuilderViewModel(IContainerExtension container, IEventAggregator eventAggr, IRegionManager regMngr, IRService rSer, IXmlConverter converter, IDataSetsManager setsManager, IProgressService progService)
        {
            xmlConverter = converter;
            rService = rSer;
            eventAggregator = eventAggr;
            regionManager = regMngr;
            containerExtension = container;
            dataSetsManager = setsManager;
            progressService = progService;

            eventAggregator.GetEvent<DataSetChangedEvent>().Subscribe(DataSetsChanged, ThreadOption.UIThread);

            ggplot = new Ggplot();

            geomControls = new ObservableCollection<VariableControl>();
            
            positionControls = new ObservableCollection<IOptionControl>();

            titleParameters = new List<Parameter>();

            Geoms = Enum.GetNames(typeof(Ggplot.Geoms)).ToList();
            Geoms.Remove("tile");

            statViewContent = new StatView();
            statViewModel = (StatViewModel)statViewContent.DataContext;

            Positions = Enum.GetNames(typeof(Ggplot.Positions)).ToList();

            NewLayerCommand = new DelegateCommand(NewLayer, CanNewLayer);
            ClearLayersCommand = new DelegateCommand(ClearLayers, CanClearLayers);
            
            SelectedGeomChangedCommand = new DelegateCommand<string>(GeomChanged);
            SelectedPositionChangedCommand = new DelegateCommand<string>(PositionChanged);
            LayerSelectedCommand = new DelegateCommand<ILayer>(LayerSelected);
            CopyChartCommand = new DelegateCommand(CopyChart);
            //ActionCommand = new DelegateCommand<string>(ExecuteActionCommand);

            currentVariables = new List<string>();

            dataSets = dataSetsManager.DataSetNames();
            if (dataSets.Count() > 0)
            {
                NewLayer();
                UpdateVariables();
                UpdateAesthetic();
                
                StatChanged(LayerSelected);
                PositionChanged(SelectedPosition);
            }
        }

        private void StatChanged(object selectedStat)
        {
            statViewModel.StatChanged("bin");
        }

        private async void PositionChanged(string obj)
        {
            var position = LoadPosition(SelectedPosition);
            UpdatePosition(position);
            SelectedLayer.Pos = position;
            await GeneratePlotAsync();
        }

        private async void GeomChanged(string obj)
        {
            UpdateAesthetic();
            await GeneratePlotAsync();
        }

      

        private void DataSetsChanged(DataSetEventArgs dataSetEventArgs)
        {
            DataSets = dataSetsManager.DataSetNames();
            NewLayerCommand.RaiseCanExecuteChanged();
        }

        private void CopyChart()
        {
            var imagePath = GetGgplotChartPath();

            Clipboard.SetImage(chartImage);
        }

        private void LayerSelected(ILayer layer)
        {
            if (layer == null)
                return;

            if (SelectedLayer != null)
                SelectedLayer.PropertyChanged -= SelectedLayer_PropertyChanged;

            SelectedLayer = layer;
            SelectedLayer.PropertyChanged += SelectedLayer_PropertyChanged;
            foreach (var vc in geomControls)
            {
                vc.PropertyChanged -= GControl_PropertyChanged;
            }
            geomControls.Clear();

            if (currentVariables == null || currentVariables.Count() == 0)
                UpdateVariables();

            foreach (var aValue in SelectedLayer.Aes.AestheticValues)
            {
                var gControl = new VariableControl(eventAggregator, aValue, currentVariables);
                gControl.PropertyChanged += GControl_PropertyChanged;
                geomControls.Add(gControl);
            }

            RaisePropertyChanged(nameof(GeomControls));
        }

        private async void SelectedLayer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(Data)))
                UpdateVariables();

            await GeneratePlotAsync();
        }

        private void UpdateVariables()
        {
            var varibleNames = dataSetsManager.DataSetVariableNames(selectedLayer.Data);
            varibleNames.Insert(0, "");
            Variables = varibleNames;
        }

        private void UpdateAesthetic()
        {
            var aesthetic = LoadAesthetic(SelectedLayer.Geom);
            //SelectedStat = aesthetic.DefaultStat;
            SelectedPosition = aesthetic.DefaultPosition;

            var aes = MergeAesthetics(aesthetic);

            foreach (var control in geomControls)
            {
                control.PropertyChanged -= GControl_PropertyChanged;
            }
            geomControls.Clear();
 
            foreach (var aValue in SelectedLayer.Aes.AestheticValues)
            {
                var gControl = new VariableControl(eventAggregator, aValue, currentVariables);
                gControl.PropertyChanged += GControl_PropertyChanged;
                geomControls.Add(gControl);
            }
        }

      

        private void UpdatePosition(Position pos)
        {
            var newControls = new ObservableCollection<IOptionControl>();

            foreach (var control in positionControls)
            {
                control.PropertyChanged -= GControl_PropertyChanged;
            }
            positionControls.Clear();

            foreach (var aProp in pos.Properties)
            {
                var oControl = new OptionPropertyControl(aProp.Tag, aProp.Name);
                oControl.SetValues(aProp.Options);
                oControl.PropertyChanged += GControl_PropertyChanged;
                newControls.Add(oControl);
            }

            foreach (var prop in pos.Booleans)
            {
                var control = new OptionCheckBoxControl(prop.Tag, prop.Name, bool.Parse(prop.Value));
                control.PropertyChanged += GControl_PropertyChanged;
                newControls.Add(control);
            }

            foreach (var prop in pos.Values)
            {
                double.TryParse(prop.Value, out double result);
                var control = new OptionValueControl(prop.Tag, prop.Name, result);
                control.PropertyChanged += GControl_PropertyChanged;
                newControls.Add(control);
            }
            PositionControls = newControls;
        }

        private Position LoadPosition(string pos)
        {
            var posXml = Properties.Resources.ResourceManager.GetObject("pos_" + pos.ToLower());
            var position = xmlConverter.ToObject<Position>(posXml.ToString());
            return position;
        }

      

        private Aesthetic LoadAesthetic(string geom)
        {
            var aestheticXml = Properties.Resources.ResourceManager.GetObject("geom_" + geom.ToLower());
            var aesthetic = xmlConverter.ToObject<Aesthetic>(aestheticXml.ToString());
            return aesthetic;
        }

        private async void GControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(ggplot.IsValid())
                await progressService.ContinueAsync(GeneratePlotAsync(), "Generating ggplot chart");
        }

        private async Task GeneratePlotAsync()
        {
            var plotCommand = ggplot.Command();

            var isPlotGenerated = await rService.GenerateGgplotAsync(plotCommand);

            if (isPlotGenerated)
            {
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
            else
                ChartImage = null;
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

        private Aesthetic MergeAesthetics(Aesthetic aestheticFromFile)
        {
            if (SelectedLayer.Aes == null)
                return null;

            var mergedAesthetic = new Aesthetic();

            mergedAesthetic.DefaultStat = aestheticFromFile.DefaultStat;
            mergedAesthetic.DefaultPosition = aestheticFromFile.DefaultPosition;

            foreach (var aesValue in aestheticFromFile.AestheticValues)
            {
                if (SelectedLayer.Aes.DoesAestheticContainValue(aesValue.Name))
                {
                    var existingValue = SelectedLayer.Aes.GetAestheticValueByName(aesValue.Name);
                    aesValue.Entry = existingValue.Entry;
                }
                mergedAesthetic.AestheticValues.Add(aesValue);
            }
            return mergedAesthetic;
            //SelectedLayer.Aes = mergedAesthetic;

            //RaisePropertyChanged(string.Empty);
        }

        private bool CanClearLayers()
        {
            return ggplot.Layers.Count > 0;
        }

        private async void ClearLayers()
        {
            Layers.Clear();
            selectedLayer = null;
            await GeneratePlotAsync();
        }

        private bool CanNewLayer()
        {
            if ((currentVariables != null || currentVariables.Count > 0) && dataSets.Any())
                return true;

            return false;
        }

        private void NewLayer()
        {
            var newLayer = new Layer("point");

            var aestheticXml = Properties.Resources.ResourceManager.GetObject("geom_point");
            var aesthetic = xmlConverter.ToObject<Aesthetic>(aestheticXml.ToString());
            newLayer.Aes = aesthetic;
            newLayer.Data = dataSets.First();

            ggplot.Layers.Add(newLayer);

            SelectedLayer = newLayer;
            LayerSelected(newLayer);
            //SelectedLayer_PropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs(nameof(Layer.Data)));

            ClearLayersCommand.RaiseCanExecuteChanged();
        }

        //private void DataSetsChanged(DataSet dataSet)
        //{
        //    PutDataSetInR(dataSet);
        //    DataSets = dataSetsManager.DataSetNames();
        //    NewLayerCommand.RaiseCanExecuteChanged();
        //}

        //public void OnNavigatedTo(NavigationContext navigationContext)
        //{
        //    if (navigationContext == null)
        //        return;

        //    var chart = (IChart)navigationContext.Parameters["chart"];

        //    layers.Add(chart.ChartLayer);

        //    ClearLayersCommand.RaiseCanExecuteChanged();
        //    LayerSelected(chart.ChartLayer);
        //    Update();
        //}

        //public bool IsNavigationTarget(NavigationContext navigationContext)
        //{
        //    return false;
        //}

        //public void OnNavigatedFrom(NavigationContext navigationContext)
        //{

        //}
    }
}

