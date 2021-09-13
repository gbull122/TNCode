using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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
        private double numClusters;

        private IEventAggregator eventAggregator;
        private IDataSetsManager dataSetsManager;
        private IProgressService progressService;

        public DelegateCommand ClusterCommand { get; private set; }

        public double NumClusters
        {
            get { return numClusters; }
            set
            {
                numClusters = value;
                RaisePropertyChanged("NumClusters");
            }
        }

        public bool UseKMeans
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

            NumClusters = 2;
            UseSpectral = true;
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

        private void Cluster()
        {
            var selectedVariables = dataSetsManager.SelectedVariables();

            var length = selectedVariables[0].Length;

            double[,] dataArray = new double[length,2];

            for (int idx = 0; idx < length; idx++)
            {
                dataArray[idx, 0] = (double)selectedVariables[0].Values.ElementAt(idx);
                dataArray[idx, 1] = (double)selectedVariables[1].Values.ElementAt(idx);
            }

            int maxClusters = (int)numClusters;

            progressService.ExecuteAsync(PerformClusteringAsync, dataArray, maxClusters);

        }


        private async Task PerformClusteringAsync(IProgress<string> progress, double[,] data, int numClusters)
        {
            progress.Report("Clustering...");
            Clusters clustering = new Clusters();


            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr pointer = handle.AddrOfPinnedObject();

            var clusters = clustering.DoCluster(pointer, 2, data.GetLength(0), numClusters, useSpectral);

            progress.Report("Sorting results...");

            var results = SortResults(clusters);

            var dataSetEventArgs = new DataSetEventArgs();
            dataSetEventArgs.Modification = DataSetChange.Added;
            dataSetEventArgs.Data = results;
            eventAggregator.GetEvent<DataSetChangedEvent>().Publish(dataSetEventArgs);

            progress.Report("Done");
        }

        public IDataSet SortResults(int[][] rawClusters)
        {
            var selectedVariables = dataSetsManager.SelectedVariables();


            object[] cluster = new object[selectedVariables[0].Length+1];
            cluster[0] = "Cluster";
            for (int clust =0;clust<rawClusters.Length;clust++)
            {
                foreach(var t in rawClusters[clust])
                {
                    cluster[t+1] = clust + 1;
                }
            }
            var var1 = new Variable(selectedVariables[0].Name, selectedVariables[0].Values.ToArray<object>());
            var var2 = new Variable(selectedVariables[1].Name, selectedVariables[1].Values.ToArray<object>());
            var clus = new Variable(cluster);

            var variables = new List<IVariable>() { var1, var2, clus };

            var dataSet = new DataSet(variables, "Clusters");
            return dataSet;
        }
    }
}
