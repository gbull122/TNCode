using Xceed.Wpf.AvalonDock;

namespace TNCodeApp.Docking
{
    public interface IDockingService
    {
        void AddAnchorable(object thing);
        void AddDocument(object thing);
        void AssignDockingManager(DockingManager dockMgr);
    }
}