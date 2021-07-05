using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TnCode.TnCodeApp.Data;
using TnCode.TnCodeApp.Data.Events;
using TnCode.TnCodeApp.Progress;

namespace Clustering.ViewModels
{
    public class RibbonViewModel : BindableBase
    {
        private bool useKmeans;
        private bool useSpectral;
        private string numClusters;

        private IEventAggregator eventAggregator;
        private IDataSetsManager dataSetsManager;
        private IProgressService progressService;

        public DelegateCommand ClusterCommand { get; private set; }

        public string NumClusters
        {
            get { return numClusters; }
            set
            {
                numClusters = value;
                RaisePropertyChanged("NumClusters");
            }
        }

        public bool UseKmeans
        {
            get { return useKmeans; }
            set
            {
                useKmeans = value;
                RaisePropertyChanged("UseKmeans");
            }
        }

        public bool UseSpectral
        {
            get { return useSpectral; }
            set
            {
                useSpectral = value;
                RaisePropertyChanged("UseSpectral");
            }
        }

        public RibbonViewModel(IEventAggregator eventAgg, IDataSetsManager dMgr, IProgressService pService)
        {
            eventAggregator = eventAgg;
            dataSetsManager = dMgr;
            progressService = pService;

            eventAggregator.GetEvent<VariableSelectionChangedEvent>().Subscribe(VariableSelectionChanged);
            ClusterCommand = new DelegateCommand(Cluster, CanCluster);
        }

        private void VariableSelectionChanged()
        {
            ClusterCommand.RaiseCanExecuteChanged();
        }

        private bool CanCluster()
        {
            var selectedVariables = dataSetsManager.SelectedVariables();

            if (selectedVariables.Count == 2 && selectedVariables[0].DataFormat == Variable.Format.Continuous && selectedVariables[1].DataFormat == Variable.Format.Continuous)
                return true;

            return false;
        }

        private async void Cluster()
        {
            var selectedVariables = dataSetsManager.SelectedVariables();

            var length = selectedVariables[0].Length;

            double[,] dataArray = new double[2, length];

           for(int idx=0;idx<length;idx++)
            {
                dataArray[0, idx] = (double)selectedVariables[0].Values.ElementAt(idx);
                dataArray[1, idx] = (double)selectedVariables[1].Values.ElementAt(idx);
            }

            await progressService.ExecuteAsync(PerformClusteringAsync, dataArray);
        }


        private async Task PerformClusteringAsync(IProgress<string> progress,double[,] data)
        {
            progress.Report("Clustering...");
            Clusters clustering = new Clusters();
            int.TryParse(numClusters, out int maxClusters);

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr pointer = handle.AddrOfPinnedObject();

            var clusters = clustering.DoCluster(pointer, 2, data.GetLength(0), maxClusters, useSpectral);
            progress.Report("Done");
        }
    }
}
