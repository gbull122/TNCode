using System.ComponentModel;

namespace TnCode.Core.R.Charts.Ggplot.Layer
{
    public interface ILayer
    {
        string Geom { get; set; }
        string Data { get; set; }
        bool ShowLegend { get; set; }
        bool ShowInPlot { get; set; }
        string Command();
        Aesthetic Aes { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        Stat Statistic { get; set; }
        Position Pos { get; set; }
    }
}
