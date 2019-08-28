using Catel.IoC;
using Catel.MVVM.Views;
using Catel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNCodeApp.Data.ViewModels;
using TNCodeApp.Data.Views;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace TNCodeApp.Docking
{
    public class DockingService : IDockingService
    {

        private DockingManager dockingManager;

        public DockingService()
        {

        }

        public void AssignDockingManager(DockingManager dockMgr)
        {
            dockingManager = dockMgr;


        }

        public void AddAnchorable(object thing)
        {
            var vm = thing as DataSetsViewModel;
            vm.InitializeViewModelAsync();

            var viewManager = ServiceLocator.Default.ResolveType<IViewManager>();


            var views = viewManager.GetViewsOfViewModel(vm);


            var anchorablePanel = new LayoutAnchorable();
            anchorablePanel.Content = thing;
            anchorablePanel.CanClose = false;

            //anchorablePanel.Closed += DocumentAnchorableClosed;

            //if (tnPanel.Docking == DockingMethod.ControlPanel)
            //{
                var controlPanel = dockingManager.FindName("ControlPanel") as LayoutAnchorablePane;
                controlPanel.Children.Add(anchorablePanel);

           // }
        }

        public void AddDocument(object thing)
        {
            throw new NotImplementedException("AddDocument");
        }
    }
}
