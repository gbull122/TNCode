using System.Collections.Generic;
using System.Text;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace TnCode.Core.R.Charts.Ggplot
{
    public class Ggplot : IRChart
    {
        public enum Geoms
        {
            bar,
            bin2d,
            boxplot,
            contour,
            density,
            density_2d,
            dotplot,
            errorbar,
            hex,
            histogram,
            line,
            point,
            quantile,
            rug,
            smooth,
            tile,
            violin
        }

        public enum Positions
        {
            dodge,
            dodge2,
            identity,
            jitter,
            stack
        }

        public enum Stats
        {
            bin,
            binhex,
            bin2d,
            bindot,
            boxplot,
            contour,
            count,
            density,
            identity,
            quantile,
            smooth,
            summary,
            ydensity
        }

        private List<ILayer> layers;

        public List<ILayer> Layers
        {
            get { return layers; }
            set { layers = value; }
        }

        public bool IsValid { get; set; }

        public Ggplot()
        {
            layers = new List<ILayer>();
        }

        public string Command()
        {
            var command = new StringBuilder();
            command.Append("p<-ggplot()");
            foreach (var layer in layers)
            {
                if (layer.ShowInPlot)
                {
                    var layerCommand = layer.Command();
                    if (!string.IsNullOrEmpty(layerCommand))
                        command.Append("+" + layerCommand);
                }
            }

            return command.ToString();
        }
    }
}
