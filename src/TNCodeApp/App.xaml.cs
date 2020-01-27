using CommonServiceLocator;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using System.Windows;
using TNCodeApp.Chart.Views;
using TNCodeApp.Docking;
using TNCodeApp.Logger;
using TNCodeApp.R.Views;
using TNCodeApp.Ribbon;
using Xceed.Wpf.AvalonDock;

namespace TNCodeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var logViewModel = new LoggerViewModel();
            containerRegistry.RegisterInstance<ILoggerFacade>(logViewModel);
            ViewModelLocationProvider.Register<LoggerView>(() => logViewModel);

            containerRegistry.RegisterForNavigation<ChartView>();

            containerRegistry.RegisterForNavigation<ChartBuilderView>();
            containerRegistry.RegisterForNavigation<GgplotTitleView>();
            containerRegistry.RegisterForNavigation<GgplotScaleView>();
            containerRegistry.RegisterForNavigation<GgplotFacetView>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            regionAdapterMappings.RegisterMapping(typeof(DockingManager), new DockingRegionAdapter(ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));

            regionAdapterMappings.RegisterMapping(typeof(Fluent.Ribbon), new RibbonRegionAdapter(ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));

            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }
    }
}
