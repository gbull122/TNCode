using Orchestra.Services;
using System.Windows;
using TNCodeApp.Ribbon.Views;
using TNCodeApp.Views;

namespace TNCodeApp.Ribbon
{
    public class RibbonService : IRibbonService
    {
        public FrameworkElement GetRibbon()
        {
            return new RibbonView();
        }

        public FrameworkElement GetMainView()
        {
            return new MainView();
        }

        public FrameworkElement GetStatusBar()
        {
            return new StatusBarView();
        }
    }
}
