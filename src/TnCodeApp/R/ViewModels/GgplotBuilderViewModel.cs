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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Media.Imaging;
using TnCode.Core.R.Charts.Ggplot;
using TnCode.Core.R.Charts.Ggplot.Layer;
using TnCode.Core.Utilities;
using TnCode.TnCodeApp.Data;
using TnCode.TnCodeApp.Data.Events;
using TnCode.TnCodeApp.Docking;
using TnCode.TnCodeApp.Progress;
using TnCode.TnCodeApp.R.Controls;

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

        //private GgplotFacetViewModel ggplotFacetViewModel;

        //public GgplotFacetViewModel FacetViewModel
        //{
        //    get { return ggplotFacetViewModel; }
        //    set
        //    {
        //        ggplotFacetViewModel = value;
        //        RaisePropertyChanged(nameof(FacetViewModel));
        //    }
        //}

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

            //ggplotFacetViewModel = new GgplotFacetViewModel();

            layers = new ObservableCollection<ILayer>();
            variableControls = new ObservableCollection<VariableControl>();
            titleParameters = new List<Parameter>();

            Geoms = Enum.GetNames(typeof(Ggplot.Geoms)).ToList();
            Geoms.Remove("tile");

            NewLayerCommand = new DelegateCommand(NewLayer, CanNewLayer);
            ClearLayersCommand = new DelegateCommand(ClearLayers, CanClearLayers);
            LayerSelectedCommand = new DelegateCommand<ILayer>(LayerSelected);
            CopyChartCommand = new DelegateCommand(CopyChart);
            //ActionCommand = new DelegateCommand<string>(ExecuteActionCommand);

            currentVariables = new List<string>();

            dataSets = dataSetsManager.DataSetNames();
            if (dataSets.Count() > 0)
                NewLayer();


        }

        private void DataSetsChanged(DataSetEventArgs dataSetEventArgs)
        {
            DataSets = dataSetsManager.DataSetNames();
            NewLayerCommand.RaiseCanExecuteChanged();
        }

        //private void ExecuteActionCommand(string obj)
        //{
        //    var navigationParameters = new NavigationParameters();

        //    OptionsView = new GgplotFacetView();

        //}


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

            if (currentVariables == null || currentVariables.Count() == 0)
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
            //ggplotFacetViewModel.Variables = varibleNames;
        }

        private void Update()
        {
            var aesthetic = LoadAesthetic(SelectedLayer.Geom);
            var aes = MergeAesthetics(aesthetic);

            var stat = LoadStat(aes.DefaultStat);
            var pos = LoadPosition(aes.DefaultPosition);

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

        private Position LoadPosition(string pos)
        {
            var posXml = Properties.Resources.ResourceManager.GetObject("pos_" + pos);
            var position = xmlConverter.ToObject<Position>(posXml.ToString());
            return position;
        }

        private Stat LoadStat(string stat)
        {
            var statXml = Properties.Resources.ResourceManager.GetObject("stat_" + stat);
            var statistic = xmlConverter.ToObject<Stat>(statXml.ToString());
            return statistic;
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

            await rService.GenerateGgplotAsync(plotCommand);

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

