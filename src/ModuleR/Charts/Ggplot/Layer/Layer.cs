using System;
using System.Text;

namespace ModuleR.Charts.Ggplot.Layer
{
    public class Layer:ILayer
    {
        public string Data { get; set; }

        public string Geom { get; set; }

        public Aesthetic Aes {get;set;}

        public string Stat
        {
            get; set;
        }

        public bool ShowLegend { get; set; }

        public string Position
        {
            get; set;
        }

        public Layer(string geom)
        {
            Geom = geom;
        }

        public string Command()
        {
            var command = new StringBuilder();

            if (Aes == null || !IsValid())
                return string.Empty;

            command.Append("layer(");
            command.Append("data=" + Data + ",");
            command.Append("geom=\"" + Geom.ToString() + "\",");
            command.Append("mapping=" + Aes.Command() + ",");
            //command.Append("stat=" + Statistic.Command() + ",");
            //command.Append("position=" + Pos.Command() + ",");
            //command.Append(ParametersCommand() + ",");
            command.Append("show.legend=" + ShowLegend.ToString().ToUpper());
            command.Append(")");
            //command.Append(DoLabels());
            //command.Append(DoScales());
            //command.Append(DoFacet());

            return command.ToString();
        }

        private bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
