using Microsoft.R.Host.Client;
using ModuleR.R;
using ModuleR.Views;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace ModuleR
{
    public class ModuleRModule : IModule
    {
        IRegionManager regionManager;
        IContainerProvider containerProvider;
        IRManager rManager;

        public ModuleRModule(IRegionManager regManager, ILoggerFacade loggerFacade)
        {
            regionManager = regManager;
            rManager = new RManager(new RHostSessionCallback(), loggerFacade);

        }

        public void OnInitialized(IContainerProvider containerPvdr)
        {
            containerProvider = containerPvdr;
           
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IRHostSessionCallback,RHostSessionCallback>();
            containerRegistry.RegisterInstance<IRManager>(rManager);
            containerRegistry.RegisterForNavigation<ChartBuilderView>();
            containerRegistry.RegisterForNavigation<LayerView>();
            regionManager.RegisterViewWithRegion("RibbonRegion", typeof(RibbonRView));
        }
    }
}
