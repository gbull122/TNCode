using AvalonDock;
using Microsoft.Extensions.Logging;
using Microsoft.R.Host.Client;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        private ManualResetEvent manualResetEvent;
        private Thread splashScreenThread;
        private SplashWindow splashWindow;
        private SplashWindowModel splashWindowModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected async override void InitializeModules()
        {
            splashWindowModel.Status = "Loading modules";
            base.InitializeModules();

           

            splashWindowModel.IsEnabled = false;
        }

        private async Task InitialiseR()
        {
            var task = Task.Run(async () =>
            {
                return await Container.Resolve<IRService>().InitialiseAsync(new Progress<string>(taskMessage =>
                {
                    splashWindowModel.Status = taskMessage;
                }));
            });
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
            // Create a new thread for the splash screen to run on
            //https://dontpaniclabs.com/blog/post/2013/11/14/dynamic-splash-screens-in-wpf/
            splashScreenThread = new Thread(ShowSplashScreen);
            splashScreenThread.SetApartmentState(ApartmentState.STA);
            splashScreenThread.IsBackground = true;
            splashScreenThread.Start();

            //Wait for the spalsh screen to show
            manualResetEvent = new ManualResetEvent(false);
            manualResetEvent.WaitOne();

            splashWindowModel.Status = "Loading";

            var logViewModel = new LoggerViewModel();
            containerRegistry.RegisterInstance<ILogger>(logViewModel);
            ViewModelLocationProvider.Register<LoggerView>(() => logViewModel);

            var path = Path.GetTempPath();
            var rHostSession = RHostSession.Create("TNCode");
            var rManager = new RManager(rHostSession, new RHostSessionCallback());
            var eventAgg = (IEventAggregator)containerRegistry.GetContainer().Resolve(typeof(IEventAggregator));
            var rService = new RService(logViewModel, rManager, path, eventAgg);
            containerRegistry.RegisterInstance<IRService>(rService);


            IChartService chartService = new ChartService(eventAgg);
            containerRegistry.RegisterInstance<IChartService>(chartService);

            var XmlConverter = new XmlConverter();
            var xmlService = new XmlService(XmlConverter, rManager);
            containerRegistry.RegisterInstance<IXmlService>(xmlService);

            containerRegistry.RegisterForNavigation<GgplotBuilderView>();

            containerRegistry.RegisterDialog<ConfirmationDialogView, ConfirmationDialogViewModel>();
            containerRegistry.RegisterDialog<NotificationDialogView, NotificationDialogViewModel>();

            InitialiseR();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            regionAdapterMappings.RegisterMapping(typeof(DockingManager), Container.Resolve<DockingRegionAdapter>());

            regionAdapterMappings.RegisterMapping(typeof(Fluent.Ribbon), Container.Resolve<RibbonRegionAdapter>());

            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = @".\" };
        }

        public void ShowSplashScreen()
        {
            splashWindowModel = new SplashWindowModel();

            splashWindow = new SplashWindow();
            splashWindow.DataContext = splashWindowModel;

            splashWindow.Show();

            //Get the main thread running again
            manualResetEvent.Set();
            System.Windows.Threading.Dispatcher.Run();
        }
    }
}
