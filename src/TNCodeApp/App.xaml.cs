using CommonServiceLocator;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using System.Windows;
using TNCodeApp.Docking;
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

        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
           
            regionAdapterMappings.RegisterMapping(typeof(DockingManager), new DockingRegionAdapter(ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));

            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }
    }
}
