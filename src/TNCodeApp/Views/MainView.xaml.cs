using Catel.IoC;
using TNCodeApp.Docking;

namespace TNCodeApp.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();


            IDockingService dc = this.GetServiceLocator().ResolveType<IDockingService>();

            if (dc != null)
            {
                dc.AssignDockingManager(dockingManager);
            }
        }
    }
}
