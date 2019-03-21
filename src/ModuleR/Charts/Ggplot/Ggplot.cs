using ModuleR.Charts.Ggplot.Layer;
using System.Collections.Generic;
using System.Text;

namespace ModuleR.Charts.Ggplot
{
    public class Ggplot:IChart
    {
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
                var layerCommand = layer.Command();
                if (!string.IsNullOrEmpty(layerCommand))
                    command.Append("+" + layerCommand);
            }

            return command.ToString();
        }
    }
}
