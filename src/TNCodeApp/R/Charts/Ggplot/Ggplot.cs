using System.Collections.Generic;
using System.Text;
using TNCodeApp.R.Charts.Ggplot.Layer;

namespace TNCodeApp.R.Charts.Ggplot
{
    public class Ggplot:IRChart
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
                if(layer.ShowInPlot)
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
