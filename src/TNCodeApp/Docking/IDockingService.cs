using Xceed.Wpf.AvalonDock;

namespace TNCodeApp.Docking
{
    public interface IDockingService
    {
        void AddAnchorable(object thing, object view);
        void AddDocument(object thing);
        void AssignDockingManager(DockingManager dockMgr);
    }
}