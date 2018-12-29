using CommonServiceLocator;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System.Windows;
using System.Windows.Controls.Ribbon;
using TNCodeApp.Data.Views;
using TNCodeApp.Docking;
using TNCodeApp.Menu;
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
            containerRegistry.RegisterForNavigation<DataView>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
           
            regionAdapterMappings.RegisterMapping(typeof(DockingManager), new DockingRegionAdapter(ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));

            regionAdapterMappings.RegisterMapping(typeof(Ribbon), new RibbonRegionAdapter(ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));

            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }
    }
}
