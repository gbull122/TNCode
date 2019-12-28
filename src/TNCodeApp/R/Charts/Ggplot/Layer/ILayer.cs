using System.ComponentModel;

namespace TNCodeApp.R.Charts.Ggplot.Layer
{
    public interface ILayer
    {
        string Geom { get; set; }
        string Data { get; set; }
        bool ShowLegend { get; set; }
        string Command();
        Aesthetic Aes { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        Stat Statistic { get; set; }
        Position Pos { get; set; }
    }
}
