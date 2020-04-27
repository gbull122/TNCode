using AvalonDock;
using CommonServiceLocator;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using System.Windows;
using TnCode.TnCodeApp.Docking;
using TnCode.TnCodeApp.Logger;
using TnCode.TnCodeApp.Ribbon;

namespace TnCode.TnCodeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private bool showGui = true;
        protected override void OnStartup(StartupEventArgs e)
        {

            //if (e.Args.Length > 0)
            //showGui = false;

            base.OnStartup(e);
        }

        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
        }
        protected override Window CreateShell()
        {
            //if (showGui)
            return Container.Resolve<MainWindow>();

            //return null;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var logViewModel = new LoggerViewModel();
            containerRegistry.RegisterInstance<ILoggerFacade>(logViewModel);
            ViewModelLocationProvider.Register<LoggerView>(() => logViewModel);

            //containerRegistry.RegisterForNavigation<ChartBuilderView>();

            //containerRegistry.RegisterDialog<ConfirmationDialogView, ConfirmationDialogViewModel>();
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
