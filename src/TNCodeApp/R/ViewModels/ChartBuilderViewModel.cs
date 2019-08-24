using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TNCode.Core.Data;
using TNCodeApp.Docking;
using TNCodeApp.R.Charts.Ggplot;
using TNCodeApp.R.Charts.Ggplot.Enums;
using TNCodeApp.R.Charts.Ggplot.Layer;
using TNCodeApp.R.Controls;

namespace TNCodeApp.R.ViewModels
{
    public class ChartBuilderViewModel : ViewModelBase
    {
        private readonly IRManager rManager;
        //private IEventAggregator eventAggregator;
        //private IRegionManager regionManager;
        private List<string> currentVariables;
        private string currentData;
        private IXmlConverter xmlConverter;
        private BitmapImage chartImage;
        private List<Parameter> titleParameters;

        public List<string> Geoms { get; }
        //public DelegateCommand NewLayerCommand { get; private set; }
        //public DelegateCommand ClearLayersCommand { get; private set; }
        //public DelegateCommand<ILayer> LayerSelectedCommand { get; private set; }
        //public DelegateCommand CopyChartCommand { get; private set; }
        //public DelegateCommand<string> ActionCommand { get; private set; }
        private ObservableCollection<VariableControl> variableControls;

        public string CurrentDataSet
        {
            get { return currentData; }
            set
            {
                currentData = value;
                RaisePropertyChanged("CurrentDataSet");
            }
        }
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

        public string Title => "Chart Builder";

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

        public ChartBuilderViewModel(IRManager rMngr, IXmlConverter converter)
        {
            xmlConverter = converter;
            rManager = rMngr;
            //eventAggregator = eventAggr;
            //regionManager = regMngr;

            //eventAggregator.GetEvent<DataSetSelectedEvent>().Subscribe(DataSetSelected, ThreadOption.UIThread);
            //eventAggregator.GetEvent<VariableControlActionEvent>().Subscribe(HandleAction);

            layers = new ObservableCollection<ILayer>();
            variableControls = new ObservableCollection<VariableControl>();
            titleParameters = new List<Parameter>();

            Geoms = Enum.GetNames(typeof(Geoms)).ToList();
            Geoms.Remove("tile");

            //NewLayerCommand = new DelegateCommand(NewLayer, CanNewLayer);
            //ClearLayersCommand = new DelegateCommand(ClearLayers, CanClearLayers);
            //LayerSelectedCommand = new DelegateCommand<ILayer>(LayerSelected);
            //CopyChartCommand = new DelegateCommand(CopyChart);
            //ActionCommand = new DelegateCommand<string>(ExecuteActionCommand);
            currentVariables = new List<string>();
            currentData = string.Empty;

        }

        private void ExecuteActionCommand(string obj)
        {
            //var navigationParameters = new NavigationParameters();

            //regionManager.RequestNavigate("OptionsRegion",
            //   new Uri("Ggplot"+obj+"View" + navigationParameters.ToString(), UriKind.Relative));
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
            if(SelectedLayer!=null)
                SelectedLayer.PropertyChanged -= SelectedLayer_PropertyChanged;

            SelectedLayer = layer;
            SelectedLayer.PropertyChanged += SelectedLayer_PropertyChanged;
            foreach (var vc in variableControls)
            {
                vc.PropertyChanged -= GControl_PropertyChanged;
            }
            variableControls.Clear();
            foreach (var aValue in SelectedLayer.Aes.AestheticValues)
            {
                //var gControl = new VariableControl(eventAggregator,aValue, currentVariables);
                //gControl.PropertyChanged += GControl_PropertyChanged;
                //variableControls.Add(gControl);
            }

            RaisePropertyChanged("VariableControls");
        }

        private async void SelectedLayer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Geom"))
            {
                Update();
            }
            await GeneratePlotAsync();
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
                //var gControl = new VariableControl(eventAggregator,aValue, currentVariables);
                //gControl.PropertyChanged += GControl_PropertyChanged;
                //variableControls.Add(gControl);
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
                    var existingValue = SelectedLayer.Aes.GetAestheticValueByName(aesValue.Name);
                    aesValue.Entry = existingValue.Entry;
                }
                mergedAesthetic.AestheticValues.Add(aesValue);
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

            //ClearLayersCommand.RaiseCanExecuteChanged();
            LayerSelected(newLayer);
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

            CurrentDataSet = dataSet.Name;

            PutDataSetInR(dataSet);
            //NewLayerCommand.RaiseCanExecuteChanged();
        }
    }
}
