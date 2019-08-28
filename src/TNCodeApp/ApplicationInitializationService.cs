using Catel;
using Catel.IoC;
using Catel.Logging;
using Orchestra.Services;
using System;
using System.Threading.Tasks;
using TNCodeApp.Data;
using TNCodeApp.Docking;

namespace TNCodeApp
{
    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private readonly IServiceLocator _serviceLocator;

        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        public override bool ShowSplashScreen => true;

        public override bool ShowShell => true;

        public ApplicationInitializationService(IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => serviceLocator);

            _serviceLocator = serviceLocator;
        }

        public override async Task InitializeBeforeCreatingShellAsync()
        {
            // Non-async first
            await RegisterTypesAsync();
            await InitializeCommandsAsync();

            await RunAndWaitAsync(new Func<Task>[]
            {
                InitializePerformanceAsync
            });
        }

        private async Task InitializeCommandsAsync()
        {
            //var commandManager = ServiceLocator.Default.ResolveType<ICommandManager>();
            //var commandInfoService = ServiceLocator.Default.ResolveType<ICommandInfoService>();

            //commandManager.CreateCommandWithGesture(typeof(Commands.Application), "Exit");
            //commandManager.CreateCommandWithGesture(typeof(Commands.Application), "About");

            //commandManager.CreateCommandWithGesture(typeof(Commands.Demo), "LongOperation");
            //commandManager.CreateCommandWithGesture(typeof(Commands.Demo), "ShowMessageBox");
            //commandManager.CreateCommandWithGesture(typeof(Commands.Demo), "Hidden");
            //commandInfoService.UpdateCommandInfo(Commands.Demo.Hidden, x => x.IsHidden = true);

            //commandManager.CreateCommand("File.Open", new InputGesture(Key.O, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            //commandManager.CreateCommand("File.SaveToImage", new InputGesture(Key.I, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            //commandManager.CreateCommand("File.Print", new InputGesture(Key.P, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);

            //var keyboardMappingsService = _serviceLocator.ResolveType<IKeyboardMappingsService>();
            //keyboardMappingsService.AdditionalKeyboardMappings.Add(new KeyboardMapping("MyGroup.Zoom", "Mousewheel", ModifierKeys.Control));

            IDataSetsManager dataSetsManager = new DataSetsManager();

        }

        public override async Task InitializeAfterCreatingShellAsync()
        {
            Log.Info("Delay to show the splash screen");

            //Thread.Sleep(2500);
        }

        private async Task InitializePerformanceAsync()
        {
            Log.Info("Improving performance");

            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }

        private async Task RegisterTypesAsync()
        {
            var serviceLocator = _serviceLocator;

            serviceLocator.RegisterType<IAboutInfoService, AboutInfoService>();

            serviceLocator.RegisterType<IDataSetsManager, DataSetsManager>(RegistrationType.Singleton);

            serviceLocator.RegisterType<IDockingService, DockingService>();
        }
    }
}
