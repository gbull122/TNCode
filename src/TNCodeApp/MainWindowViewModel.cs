using CommonServiceLocator;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using TNCodeApp.Menu.Views;
using Unity;

namespace TNCodeApp
{
    public class MainWindowViewModel : BindableBase
    {

        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;
        private IUnityContainer container;
        private string title = "TNCode";

        public MainWindowViewModel(IUnityContainer contain, IRegionManager regManager)
        {
            container = contain;
            regionManager = regManager;

            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            //container.RegisterType<IGgplotRibbon, GgplotRibbon>(new ContainerControlledLifetimeManager());

            //container.RegisterType<IDataService, DataService>(new ContainerControlledLifetimeManager());



            regionManager.RegisterViewWithRegion("MenuRegion", typeof(MenuView));
            //regionManager.RegisterViewWithRegion("DataRegion", typeof(DataView));

            ////regionManager.RegisterViewWithRegion("ChartRegion", typeof(ChartBuilderView));

            //eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEventAggregator>();
            //eventAggregator.GetEvent<DataSelectedEvent>().Subscribe(DataSelected);
        }
    }
}
