using Clustering.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Clustering
{
    public class Module : IModule
    {

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("RibbonRegion", typeof(RibbonView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
