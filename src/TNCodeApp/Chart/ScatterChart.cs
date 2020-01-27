using System.Collections.Generic;
using TNCode.Core.Data;
using TNCodeApp.R.Charts.Ggplot;
using TNCodeApp.R.Charts.Ggplot.Layer;

namespace TNCodeApp.Chart
{
    public class ScatterChart : IChart
    {
        private string title;
        private Dictionary<string, ICollection<string>> data;
        private Ggplot ggplot;
        private IXmlConverter xmlConverter;
        private Layer layer;


        public string Title { get => title; set => title = value; }

        public Dictionary<string, ICollection<string>> Data { get => data; set => data = value; }

        public string DataSetName
        {
            get;set;
        }

        public Ggplot Plot
        {
            get => ggplot;
            set
            {
                ggplot = value;
            }
        }

        public Layer ChartLayer
        {
            get => layer;
        }

        public IXmlConverter Converter { set => xmlConverter=value; }

        public ScatterChart(Dictionary<string, ICollection<string>> variableList)
        {
            Data = variableList;
        }

        public void Update()
        {
            ggplot = new Ggplot();

            layer = new Layer("point");

            var aestheticXml = Properties.Resources.ResourceManager.GetObject("geom_point");
            var aesthetic = xmlConverter.ToObject<Aesthetic>(aestheticXml.ToString());
            layer.Aes = aesthetic;

            ggplot.Layers.Add(layer);
        }

        public bool CanPlot()
        {
            return SelectionContainsVariables()==2 && SelectionFromSingleDataSet();
        }

        public bool SelectionFromSingleDataSet()
        {
            return data.Count == 1;
        }

        public int SelectionContainsVariables()
        {
            var totalVariables = 0;
            foreach(var entry in data)
            {
                totalVariables += entry.Value.Count;
            }
            return totalVariables;
        }
    }
}
