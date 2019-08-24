using Catel.IoC;
using Catel.Logging;
using Catel.Services;
using Orchestra.Services;
using Orchestra.Views;
using System.Globalization;
using System.Windows;
using TNCodeApp.Ribbon;

namespace TNCodeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            var languageService = ServiceLocator.Default.ResolveType<ILanguageService>();

            // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
            // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
            // we use .CurrentCulture for the sake of the demo
            languageService.PreferredCulture = CultureInfo.CurrentCulture;
            languageService.FallbackCulture = new CultureInfo("en-US");

            //FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orchestra.Examples.Ribbon.Fluent;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));

            //FontImage.DefaultFontFamily = "FontAwesome";

            var serviceLocator = ServiceLocator.Default;

            serviceLocator.RegisterType<IRibbonService, RibbonService>();
            serviceLocator.RegisterType<IApplicationInitializationService, ApplicationInitializationService>();

            var shellService = serviceLocator.ResolveType<IShellService>();
            shellService.CreateAsync<ShellWindow>();
            

        }

        //protected override void RegisterTypes(IContainerRegistry containerRegistry)
        //{
        //    containerRegistry.Register<ILoggerFacade, TnLogger>();

        //    containerRegistry.RegisterForNavigation<ChartView>();

        //    containerRegistry.RegisterForNavigation<ChartBuilderView>();
        //    containerRegistry.RegisterForNavigation<GgplotTitleView>();
        //    containerRegistry.RegisterForNavigation<GgplotScaleView>();
        //    containerRegistry.RegisterForNavigation<GgplotFacetView>();
        //}

        //protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        //{
        //    regionAdapterMappings.RegisterMapping(typeof(DockingManager), new DockingRegionAdapter(ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));

        //    regionAdapterMappings.RegisterMapping(typeof(Fluent.Ribbon), new RibbonRegionAdapter(ServiceLocator.Current.GetInstance<IRegionBehaviorFactory>()));

        //    base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        //}

    }
}
