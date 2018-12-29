using ModuleR.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace ModuleR
{
    public class ModuleRModule : IModule
    {
        IRegionManager regionManager;

        public ModuleRModule(IRegionManager rManager)
        {
            regionManager = rManager;

            regionManager.RegisterViewWithRegion("RibbonRegion", typeof(RibbonRView));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
