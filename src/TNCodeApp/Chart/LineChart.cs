using System.Collections.Generic;
using TNCode.Core.Data;
using TNCodeApp.R.Charts.Ggplot;
using TNCodeApp.R.Charts.Ggplot.Layer;

namespace TNCodeApp.Chart
{
    public class LineChart : IChart
    {
        private string title;
        private IList<string> data;
        private Ggplot ggplot;
        private readonly IXmlConverter xmlConverter;

        public string Title { get => title; set => title=value; }

        public IList<string> Data { get => data; set => data = value; }

        public string DataSetName
        {
            get; set;
        }

        public Ggplot Plot
        {
            get => ggplot;
            set
            {
                ggplot = value;
            }
        }

        public LineChart(IList<string> variableList)
        {
            ggplot = new Ggplot();

            var newLayer = new Layer("line");

            var aestheticXml = Properties.Resources.ResourceManager.GetObject("geom_line");
            var aesthetic = xmlConverter.ToObject<Aesthetic>(aestheticXml.ToString());
            newLayer.Aes = aesthetic;

            ggplot.Layers.Add(newLayer);
        }

        public void Update()
        {
            
        }

        public bool CanPlot()
        {
            return data.Count == 1;
        }
    }
}
