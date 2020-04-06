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
using TNCode.Core.Data;
using TNCodeApp.Chart;
using TNCodeApp.Data;
using TNCodeApp.Data.Events;
using TNCodeApp.Docking;
using TNCodeApp.Progress;
using TNCodeApp.R.Charts.Ggplot;
using TNCodeApp.R.Charts.Ggplot.Enums;
using TNCodeApp.R.Charts.Ggplot.Layer;
using TNCodeApp.R.Controls;
using TNCodeApp.R.Events;
using TNCodeApp.R.Views;

namespace TNCodeApp.R.ViewModels
{
    public class ChartBuilderViewModel : BindableBase, ITnPanel, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IContainerExtension containerExtension;

        private readonly IRManager rManager;
        private readonly IDataSetsManager dataSetsManager;

        private readonly IProgressService progressService;

        private List<string> currentVariables;
        private IEnumerable<string> dataSets;
        private readonly IXmlConverter xmlConverter;
        private BitmapImage chartImage;
        private List<Parameter> titleParameters;

        public List<string> Geoms { get; }
        public DelegateCommand NewLayerCommand { get; private set; }
        public DelegateCommand ClearLayersCommand { get; private set; }
        public DelegateCommand<ILayer> LayerSelectedCommand { get; private set; }
        public DelegateCommand CopyChartCommand { get; private set; }
        public DelegateCommand<string> ActionCommand { get; private set; }

        private ObservableCollection<VariableControl> variableControls;

        private GgplotFacetView facetView;
        private GgplotScaleView scaleView;
        private GgplotTitleView titleView;
        private GgplotStatView statView;

        public object OptionsView
        {
            get { return optionsView; }
            set
            {
                optionsView = value;
                RaisePropertyChanged(nameof(OptionsView));
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

        public ObservableCollection<VariableControl> VariableControls
        {
            get { return variableControls; }
            set
            {
                variableControls = value;
                RaisePropertyChanged(nameof(VariableControls));
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

        public string Title => "Chart Builder";

        public DockingMethod Docking => DockingMethod.Document;

        private ILayer selectedLayer;

        public ILayer SelectedLayer
        {
            get => selectedLayer;
            set
            {
                selectedLayer = value;
                RaisePropertyChanged(nameof(SelectedLayer));
            }
        }

        private ObservableCollection<ILayer> layers;
        private object optionsView;

        public ObservableCollection<ILayer> Layers
        {
            get { return layers; }
            set
            {
                layers = value;
                RaisePropertyChanged(nameof(Layers));
            }
        }

        public ChartBuilderViewModel(IContainerExtension container, IEventAggregator eventAggr, IRegionManager regMngr, IRManager rMngr, IXmlConverter converter, IDataSetsManager setsManager, IProgressService progService)
        {
            xmlConverter = converter;
            rManager = rMngr;
            eventAggregator = eventAggr;
            regionManager = regMngr;
            containerExtension = container;
            dataSetsManager = setsManager;
            progressService = progService;

            eventAggregator.GetEvent<DataSetChangedEvent>().Subscribe(DataSetsChanged, ThreadOption.UIThread);

            eventAggregator.GetEvent<VariableControlActionEvent>().Subscribe(HandleAction);

            layers = new ObservableCollection<ILayer>();
            variableControls = new ObservableCollection<VariableControl>();
            titleParameters = new List<Parameter>();

            Geoms = Enum.GetNames(typeof(Geoms)).ToList();
            Geoms.Remove("tile");

            NewLayerCommand = new DelegateCommand(NewLayer, CanNewLayer);
            ClearLayersCommand = new DelegateCommand(ClearLayers, CanClearLayers);
            LayerSelectedCommand = new DelegateCommand<ILayer>(LayerSelected);
            CopyChartCommand = new DelegateCommand(CopyChart);
            ActionCommand = new DelegateCommand<string>(ExecuteActionCommand);

            currentVariables = new List<string>();

            dataSets = dataSetsManager.DataSetNames();
            if (dataSets.Count() > 0)
                NewLayer();

            //var view = containerExtension.Resolve<GgplotFacetEvent>();
            //IRegion region = regionManager.Regions["FacetRegion"];
            //region.Add(view);
        }

        private void DataSetsChanged(DataSetEventArgs dataSetEventArgs)
        {
            DataSets = dataSetsManager.DataSetNames();
            NewLayerCommand.RaiseCanExecuteChanged();
        }

        private void ExecuteActionCommand(string obj)
        {
            var navigationParameters = new NavigationParameters();

            OptionsView = new GgplotFacetView();

        }

        private void HandleAction(string aestheticName)
        {

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
            foreach (var vc in variableControls)
            {
                vc.PropertyChanged -= GControl_PropertyChanged;
            }
            variableControls.Clear();

            if (currentVariables == null || currentVariables.Count()==0)
                UpdateVariables();

            foreach (var aValue in SelectedLayer.Aes.AestheticValues)
            {
                var gControl = new VariableControl(eventAggregator, aValue, currentVariables);
                gControl.PropertyChanged += GControl_PropertyChanged;
                variableControls.Add(gControl);
            }

            RaisePropertyChanged(nameof(VariableControls));
        }

        private async void SelectedLayer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(Layer.Data)))
            {
                UpdateVariables();
            }

            if (e.PropertyName.Equals(nameof(Layer.Geom)))
                Update();

            await GeneratePlotAsync();
        }

        private void UpdateVariables()
        {
            var varibleNames = dataSetsManager.DataSetVariableNames(selectedLayer.Data);
            varibleNames.Insert(0, "");
            currentVariables = varibleNames;
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
                var gControl = new VariableControl(eventAggregator, aValue, currentVariables);
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

            await progressService.ContinueAsync(GeneratePlotAsync(), "Generating ggplot chart");
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
                    var existingValue = SelectedLayer.Aes.GetAestheticValueByName(aesValue.Name);
                    aesValue.Entry = existingValue.Entry;
                }
                mergedAesthetic.AestheticValues.Add(aesValue);
            }
            SelectedLayer.Aes = mergedAesthetic;

            RaisePropertyChanged(string.Empty);
        }

        private bool CanClearLayers()
        {
            return layers.Count > 0;
        }

        private void ClearLayers()
        {
            Layers.Clear();
            selectedLayer = null;
            Update();
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

            layers.Add(newLayer);

            LayerSelected(newLayer);
            SelectedLayer_PropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs(nameof(Layer.Data)));

            ClearLayersCommand.RaiseCanExecuteChanged();
        }

        //private void DataSetsChanged(DataSet dataSet)
        //{
        //    PutDataSetInR(dataSet);
        //    DataSets = dataSetsManager.DataSetNames();
        //    NewLayerCommand.RaiseCanExecuteChanged();
        //}

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext == null)
                return;

            var chart = (IChart)navigationContext.Parameters["chart"];

            layers.Add(chart.ChartLayer);

            ClearLayersCommand.RaiseCanExecuteChanged();
            LayerSelected(chart.ChartLayer);
            Update();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
