using Prism.Regions;
using System.Windows;
using System.Windows.Controls;

namespace TNCodeApp.R.Views
{
    /// <summary>
    /// Interaction logic for GgplotView.xaml
    /// </summary>
    public partial class ChartBuilderView : UserControl
    {
        public ChartBuilderView(IRegionManager regionManager)
        {
            InitializeComponent();
            if (regionManager != null)
            {
                SetRegionManager(regionManager, Options, "OptionsRegion");
            }
        }

        public void SetRegionManager(IRegionManager manager, DependencyObject regionTarget, string regionName)

        {

            RegionManager.SetRegionName(regionTarget, regionName);

            RegionManager.SetRegionManager(regionTarget, manager);

        }
    }
}
