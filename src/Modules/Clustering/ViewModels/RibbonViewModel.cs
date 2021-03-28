using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Clustering.ViewModels
{
    public class RibbonViewModel : BindableBase
    {
        private bool useKmeans;
        private bool useSpectral;
        private string numClusters;

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

        public RibbonViewModel()
        {
            ClusterCommand = new DelegateCommand(Cluster, CanCluster);
        }

        private bool CanCluster()
        {
            return false;
        }

        private void Cluster()
        {
            
        }

        private async Task PerformClusteringAsync()
        {
            Clusters clustering = new Clusters();
            //int.TryParse(numClusters, out int maxClusters);

            //GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            //IntPtr pointer = handle.AddrOfPinnedObject();

            //var clusters = clustering.DoCluster(pointer, 2, data.GetLength(0), maxClusters, useSpectral);
        }
    }
}
