using AvalonDock;
using CommonServiceLocator;
using Microsoft.R.Host.Client;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using System.IO;
using System.Windows;
using TnCode.Core.R;
using TnCode.Core.R.Charts.Ggplot;
using TnCode.Core.Utilities;
using TnCode.TnCodeApp.Charts;
using TnCode.TnCodeApp.Docking;
using TnCode.TnCodeApp.Logger;
using TnCode.TnCodeApp.R;
using TnCode.TnCodeApp.R.Views;
using TnCode.TnCodeApp.Ribbon;
using Unity;

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
            base.OnStartup(e);
        }

        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var logViewModel = new LoggerViewModel();
            containerRegistry.RegisterInstance<ILoggerFacade>(logViewModel);
            ViewModelLocationProvider.Register<LoggerView>(() => logViewModel);

            var path = Path.GetTempPath();
            var rHostSession = RHostSession.Create("TNCode");
            var rManager = new RManager(rHostSession, new RHostSessionCallback());
            var eventAgg = (IEventAggregator)containerRegistry.GetContainer().Resolve(typeof(IEventAggregator));
            var rService = new RService(logViewModel, rManager,path, eventAgg);
            containerRegistry.RegisterInstance<IRService>(rService);

            IChartService chartService = new ChartService(eventAgg);
            containerRegistry.RegisterInstance<IChartService>(chartService);

            var XmlConverter = new XmlConverter();
            var xmlService = new XmlService(XmlConverter, rManager);
            containerRegistry.RegisterInstance<IXmlService>(xmlService);

            containerRegistry.RegisterForNavigation<GgplotBuilderView>();

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
