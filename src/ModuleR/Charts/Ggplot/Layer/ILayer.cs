﻿namespace ModuleR.Charts.Ggplot.Layer
{
    public interface ILayer
    {
        string Geom { get; set; }
        string Data { get; set; }
        bool ShowLegend { get; set; }
        string Command();
        Aesthetic Aes { get; set; }
    }
}
