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
using TnCode.TnCodeApp.Data;
using TnCode.TnCodeApp.Data.Events;
using TnCode.TnCodeApp.Docking;
using TnCode.TnCodeApp.Progress;
using TnCode.TnCodeApp.R.Views;

namespace TnCode.TnCodeApp.R.ViewModels
{
    public class GgplotBuilderViewModel:BindableBase, IDockingPanel
    {
        private const string DEFAULT_GEOM = "point";

        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IContainerExtension containerExtension;

        private readonly IRService rService;
        private readonly IDataSetsManager dataSetsManager;

        private readonly IProgressService progressService;
        private readonly IXmlService xmlService;

        private Ggplot ggplot;

        private IEnumerable<string> dataSets;

        private BitmapImage chartImage;
        
        public string Title => "Chart Builder";

        public DockingMethod Docking => DockingMethod.Document;

        public DelegateCommand NewLayerCommand { get; private set; }
        public DelegateCommand ClearLayersCommand { get; private set; }
        public DelegateCommand<ILayer> LayerSelectedCommand { get; private set; }
        public DelegateCommand CopyChartCommand { get; private set; }

        public DelegateCommand<string> GeomSelectedCommand { get; private set; }
        public DelegateCommand<string> DataSelectedCommand { get; private set; }

        private StatViewModel statViewModel;
        private GeomViewModel geomViewModel;
        private PositionViewModel positionViewModel;
        private FacetViewModel facetViewModel;
        private TitlesViewModel titlesViewModel;

        public IEnumerable<string> DataSets
        {
            get { return dataSets; }
            set
            {
                dataSets = value;
                RaisePropertyChanged(nameof(DataSets));
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

        private ILayer selectedLayer;
        private List<ILayer> layers = new List<ILayer>();

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

        public ObservableCollection<ILayer> Layers
        {
            get { return ggplot.Layers; }
        }

        public List<string> Geoms { get; }

        public GgplotBuilderViewModel(IContainerExtension container, IEventAggregator eventAggr, IRegionManager regMngr, IRService rSer, IDataSetsManager setsManager, IProgressService progService, IXmlService xml)
        {
            rService = rSer;
            eventAggregator = eventAggr;
            regionManager = regMngr;
            containerExtension = container;
            dataSetsManager = setsManager;
            progressService = progService;
            xmlService = xml;

            //TODO
            ggplot = new Ggplot();

            eventAggregator.GetEvent<DataSetChangedEvent>().Subscribe(DataSetsChanged, ThreadOption.UIThread);

            NewLayerCommand = new DelegateCommand(NewLayer, CanNewLayer);
            ClearLayersCommand = new DelegateCommand(ClearLayers, CanClearLayers);
            LayerSelectedCommand = new DelegateCommand<ILayer>(LayerSelected);
            CopyChartCommand = new DelegateCommand(CopyChart);
            GeomSelectedCommand = new DelegateCommand<string>(GeomSelected);
            DataSelectedCommand = new DelegateCommand<string>(DataSelected);
            dataSets = dataSetsManager.DataSetNames();


            Geoms = Enum.GetNames(typeof(Ggplot.Geoms)).ToList();
            Geoms.Remove("tile");
        }

        private void DataSelected(string DataSetName)
        {
            geomViewModel.Variables = UpdateVariables();
        }

        private void GeomSelected(string geom)
        {
            var aes = xmlService.LoadAesthetic(geom);
            var stat = xmlService.LoadStat(aes.DefaultStat);
            var pos = xmlService.LoadPosition(aes.DefaultPosition);

            selectedLayer.Aes = aes;
            selectedLayer.Statistic = stat;
            selectedLayer.Pos = pos;

            geomViewModel.SetAesthetics(aes);
            geomViewModel.SetControls();

            statViewModel.SetStat(stat);
            statViewModel.SetControls();

            positionViewModel.SetPosition(pos);
            positionViewModel.SetControls();
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

        private void SetViewModels()
        {
            IRegion geomRegion = regionManager.Regions["GeomRegion"];
            var geomView = new GeomView();
            geomRegion.Add(geomView, "GeomView", true);
            geomViewModel = (GeomViewModel)geomView.DataContext;
            geomViewModel.GeomChanged += GeomViewModel_GeomChanged;

            IRegion statRegion = regionManager.Regions["StatRegion"];
            var statView = new StatView();
            statRegion.Add(statView, "StatView", true);
            statViewModel = (StatViewModel)statView.DataContext;

            IRegion posRegion = regionManager.Regions["PositionRegion"];
            var posView = new PositionView();
            posRegion.Add(posView, "PositionView", true);
            positionViewModel = (PositionViewModel)posView.DataContext;
        }

        private void NewLayer()
        {
            if(Layers.Count==0)
                SetViewModels();

            var aes = xmlService.LoadAesthetic(DEFAULT_GEOM);
            var stat = xmlService.LoadStat(aes.DefaultStat);
            var pos = xmlService.LoadPosition(aes.DefaultPosition);

            var newLayer = new Layer(DEFAULT_GEOM, aes, stat, pos);
            newLayer.Data = dataSets.First();

            Layers.Add(newLayer);

            LayerSelected(newLayer);

            ClearLayersCommand.RaiseCanExecuteChanged();
        }

        private void LayerSelected(ILayer layer)
        {
            if (layer == null || selectedLayer==layer)
                return;

            if (selectedLayer != null)
                selectedLayer.PropertyChanged -= SelectedLayer_PropertyChanged;

            SelectedLayer = layer;

            geomViewModel.Variables = UpdateVariables();
            geomViewModel.SetAesthetics(layer.Aes);
            geomViewModel.SetControls();

            statViewModel.SetStat(layer.Statistic);
            statViewModel.SetControls();

            positionViewModel.SetPosition(layer.Pos);
            positionViewModel.SetControls();

            selectedLayer.PropertyChanged += SelectedLayer_PropertyChanged;
        }

        private async void GeomViewModel_GeomChanged(object sender, EventArgs e)
        {
            if (ggplot.IsValid())
                await progressService.ContinueAsync(GeneratePlotAsync(), "Generating ggplot chart");
        }

        private async void SelectedLayer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ggplot.IsValid())
                await progressService.ContinueAsync(GeneratePlotAsync(), "Generating ggplot chart");
        }

        private List<string> UpdateVariables()
        {
            var varibleNames = dataSetsManager.DataSetVariableNames(selectedLayer.Data);
            varibleNames.Insert(0, "");
            return varibleNames;
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

        private bool CanClearLayers()
        {
            return Layers.Count > 0;
        }

        private async void ClearLayers()
        {
            Layers.Clear();
            selectedLayer = null;
            await progressService.ContinueAsync(GeneratePlotAsync(), "Clearing ggplot chart");
        }

        private bool CanNewLayer()
        {
            if ( dataSets.Any())
                return true;

            return false;
        }
    }
}

