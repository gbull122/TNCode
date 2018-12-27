namespace TNCodeApp.Docking
{
    public interface ITnPanel
    {
        string Title { get;}

        DockingMethod Docking { get; }
    }
}
