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

        private IEnumerable<string> dataSets;

        private BitmapImage chartImage;
        
        public string Title => "Chart Builder";

        public DockingMethod Docking => DockingMethod.Document;

        public DelegateCommand NewLayerCommand { get; private set; }
        public DelegateCommand ClearLayersCommand { get; private set; }
        public DelegateCommand<ILayer> LayerSelectedCommand { get; private set; }
        public DelegateCommand CopyChartCommand { get; private set; }


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

        public List<ILayer> Layers
        {
            get { return ggplot.Layers; }

        }

        public GgplotBuilderViewModel(IContainerExtension container, IEventAggregator eventAggr, IRegionManager regMngr, IRService rSer, IDataSetsManager setsManager, IProgressService progService)
        {
            rService = rSer;
            eventAggregator = eventAggr;
            regionManager = regMngr;
            containerExtension = container;
            dataSetsManager = setsManager;
            progressService = progService;

            eventAggregator.GetEvent<DataSetChangedEvent>().Subscribe(DataSetsChanged, ThreadOption.UIThread);

            ggplot = new Ggplot();

            IRegion region = regionManager.Regions["MainRegion"];

            NewLayerCommand = new DelegateCommand(NewLayer, CanNewLayer);
            ClearLayersCommand = new DelegateCommand(ClearLayers, CanClearLayers);
            LayerSelectedCommand = new DelegateCommand<ILayer>(LayerSelected);
            CopyChartCommand = new DelegateCommand(CopyChart);

            dataSets = dataSetsManager.DataSetNames();
            if (dataSets.Count() > 0)
            {
                NewLayer();
                UpdateVariables();
                UpdateAesthetic();
               
            }
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
            //StatChanged(SelectedLayer.Statistic);

            //SelectedLayer.PropertyChanged += SelectedLayer_PropertyChanged;
            //foreach (var vc in geomControls)
            //{
            //    vc.PropertyChanged -= GControl_PropertyChanged;
            //}
            //geomControls.Clear();

            //if (currentVariables == null || currentVariables.Count() == 0)
            //    UpdateVariables();

            //foreach (var aValue in SelectedLayer.Aes.AestheticValues)
            //{
            //    var gControl = new VariableControl(eventAggregator, aValue, currentVariables);
            //    gControl.PropertyChanged += GControl_PropertyChanged;
            //    geomControls.Add(gControl);
            //}

            //RaisePropertyChanged(nameof(GeomControls));
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
           // Variables = varibleNames;
        }

        private void UpdateAesthetic()
        {
            //var aesthetic = LoadAesthetic(SelectedLayer.Geom);
            ////SelectedStat = aesthetic.DefaultStat;
            //SelectedPosition = aesthetic.DefaultPosition;

            //var aes = MergeAesthetics(aesthetic);

            //foreach (var control in geomControls)
            //{
            //    control.PropertyChanged -= GControl_PropertyChanged;
            //}
            //geomControls.Clear();
 
            //foreach (var aValue in SelectedLayer.Aes.AestheticValues)
            //{
            //    var gControl = new VariableControl(eventAggregator, aValue, currentVariables);
            //    gControl.PropertyChanged += GControl_PropertyChanged;
            //    geomControls.Add(gControl);
            //}
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
            //if ((currentVariables != null || currentVariables.Count > 0) && dataSets.Any())
                return true;

            //return false;
        }

        private void NewLayer()
        {
            //var newLayer = new Layer("point");
            //newLayer.Aes = LoadAesthetic("point");
            //newLayer.Data = dataSets.First();

            //var stat = LoadStat(newLayer.Aes.DefaultStat);
            //newLayer.Statistic = stat;

            ////var pos= LoadPosition(newLayer.Aes.DefaultPosition);
            ////newLayer.Pos = pos;

            //ggplot.Layers.Add(newLayer);

            //SelectedLayer = newLayer;
            //LayerSelected(newLayer);

            //ClearLayersCommand.RaiseCanExecuteChanged();
        }

    }
}

