﻿using Prism.Commands;
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
        private AestheticViewModel aesViewModel;
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
            selectedLayer.Data = DataSetName;
            aesViewModel.UpdateVariables(UpdateVariables(DataSetName));

        }

        private void GeomSelected(string geom)
        {
            var aes = xmlService.LoadAesthetic(geom);
            var stat = xmlService.LoadStat(aes.DefaultStat);
            var pos = xmlService.LoadPosition(aes.DefaultPosition);

            selectedLayer.Geom = geom;
            selectedLayer.Aes = aes;
            selectedLayer.Statistic = stat;
            selectedLayer.Pos = pos;

            aesViewModel.SetAesthetic(aes);
            statViewModel.SetStat(stat);
            positionViewModel.SetPosition(pos);
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
            IRegion aesRegion = regionManager.Regions["AestheticRegion"];
            var aesView = new AestheticView();
            aesRegion.Add(aesView, Guid.NewGuid().ToString(), true);
            aesViewModel = (AestheticViewModel)aesView.DataContext;
            aesViewModel.AesChanged += ViewModel_Changed;

            IRegion statRegion = regionManager.Regions["StatRegion"];
            var statView = new StatView();
            statRegion.Add(statView, Guid.NewGuid().ToString(), true);
            statViewModel = (StatViewModel)statView.DataContext;
            statViewModel.StatChanged += ViewModel_Changed;

            IRegion posRegion = regionManager.Regions["PositionRegion"];
            var posView = new PositionView();
            posRegion.Add(posView, Guid.NewGuid().ToString(), true);
            positionViewModel = (PositionViewModel)posView.DataContext;
            positionViewModel.PositionChanged += ViewModel_Changed;

            IRegion titlesRegion = regionManager.Regions["TitlesRegion"];
            var titlesView = new TitlesView();
            titlesRegion.Add(titlesView, "TitlesView", true);
            titlesViewModel = (TitlesViewModel)titlesView.DataContext;
            titlesViewModel.PropertyChanged += ViewModel_Changed;

            IRegion facetRegion = regionManager.Regions["FacetRegion"];
            var facetView = new FacetView();
            facetRegion.Add(facetView, "FacetView", true);
            facetViewModel = (FacetViewModel)facetView.DataContext;
            facetViewModel.PropertyChanged+= ViewModel_Changed;
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

            var variables = UpdateVariables(SelectedLayer.Data);
            aesViewModel.Variables = variables;
            aesViewModel.SetAesthetic(layer.Aes);
            statViewModel.SetStat(layer.Statistic);
            positionViewModel.SetPosition(layer.Pos);

            facetViewModel.Variables = variables;

            selectedLayer.PropertyChanged += SelectedLayer_PropertyChanged;
        }

        private async void ViewModel_Changed(object sender, EventArgs e)
        {
            if (ggplot.IsValid())
                await progressService.ContinueAsync(GeneratePlotAsync(), "Generating ggplot chart");
        }

        private async void SelectedLayer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ggplot.IsValid())
                await progressService.ContinueAsync(GeneratePlotAsync(), "Generating ggplot chart");
        }

        private List<string> UpdateVariables(string dataSet)
        {
            var varibleNames = dataSetsManager.DataSetVariableNames(dataSet);
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

