using AvalonDock;
using Microsoft.Extensions.Logging;
using Microsoft.R.Host.Client;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using System.IO;
using System.Windows;
using TnCode.Core.R;
using TnCode.Core.Utilities;
using TnCode.TnCodeApp.Charts;
using TnCode.TnCodeApp.Dialogs.ViewModels;
using TnCode.TnCodeApp.Dialogs.Views;
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
            containerRegistry.RegisterInstance<ILogger>(logViewModel);
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

            containerRegistry.RegisterDialog<ConfirmationDialogView, ConfirmationDialogViewModel>();
            containerRegistry.RegisterDialog<NotificationDialogView, NotificationDialogViewModel>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            regionAdapterMappings.RegisterMapping(typeof(DockingManager), Container.Resolve<DockingRegionAdapter>());

            regionAdapterMappings.RegisterMapping(typeof(Fluent.Ribbon), Container.Resolve<RibbonRegionAdapter>());

            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
        }
    }
}
