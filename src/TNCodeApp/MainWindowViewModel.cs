using CommonServiceLocator;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using TNCodeApp.Data.Views;
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

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public MainWindowViewModel(IUnityContainer contain, IRegionManager regManager)
        {
            container = contain;
            regionManager = regManager;

            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            regionManager.RegisterViewWithRegion("RibbonRegion", typeof(RibbonView));
            regionManager.RegisterViewWithRegion("RibbonRegion", typeof(DataRibbonView));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(DataSetsView));
        }
    }
}
