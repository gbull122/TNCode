namespace ModuleR.Charts.Ggplot.Layer
{
    public interface ILayer
    {
        string Geom { get; set; }
        string Command();
        Aesthetic Aes { get; set; }
    }
}
