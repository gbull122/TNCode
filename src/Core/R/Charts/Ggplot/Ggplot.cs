using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TnCode.Core.R.Charts.Ggplot.Layer;

namespace TnCode.Core.R.Charts.Ggplot
{
    public interface IGgplot
    {
        void AddLayer(ILayer layer);
        string Command();
        bool IsValid();
        ObservableCollection<ILayer> Layers { get; }
        void UpdateFacet(Facet newFacet);
        void UpdateTitles(List<Parameter> labels);
    }

    public class Ggplot : IRChart, IGgplot
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

        private List<Parameter> labels;

        private ObservableCollection<ILayer> layers;

        private Facet facet;

        public void AddLayer(ILayer layer)
        {
            layers.Add(layer);
        }

        public ObservableCollection<ILayer> Layers
        {
            get { return layers; }
        }

        public bool IsValid()
        {
            foreach (var layer in layers)
            {
                if (layer.ShowInPlot && !layer.IsValid())
                {
                    return false;
                }
            }
            return true;
        }

        public Ggplot()
        {
            layers = new ObservableCollection<ILayer>();
            labels = new List<Parameter>();
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

            command.Append(DoFacet());
            command.Append(DoLabels());

            return command.ToString();
        }

        private string DoFacet()
        {
            if (facet == null)
                return string.Empty;

            return facet.PlotCommand();
        }

        private string DoLabels()
        {
            List<string> titles = new List<string>();
            if (labels.Count == 0)
                return string.Empty;

            foreach (var l in labels)
            {
                if (!string.IsNullOrEmpty(l.Command()))
                    titles.Add(l.Command());
            }

            if (titles.Count == 0)
                return string.Empty;

            return "+labs(" + string.Join(",", titles) + ")";
        }

        public void UpdateFacet(Facet newFacet)
        {
            facet = newFacet;
        }

        public void UpdateTitles(List<Parameter> newLabels)
        {
            labels = newLabels;
        }
    }
}
