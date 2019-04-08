using Prism.Mvvm;
using TNCodeApp.Docking;

namespace TNCodeApp.Progress
{
    public class ProgressViewModel : BindableBase, ITnPanel
    {
        public string Title { get => "Status"; }

        public DockingMethod Docking { get => DockingMethod.StatusPanel; }

        public ProgressViewModel()
        {

        }
    }
}
