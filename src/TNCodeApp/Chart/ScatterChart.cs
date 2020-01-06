using System.Collections.Generic;
using TNCode.Core.Data;
using TNCodeApp.R.Charts.Ggplot;
using TNCodeApp.R.Charts.Ggplot.Layer;

namespace TNCodeApp.Chart
{
    public class ScatterChart : IChart
    {
        private string title;
        private IList<string> data;
        private Ggplot ggplot;
        private readonly IXmlConverter xmlConverter;

        public string Title { get => title; set => title = value; }

        public IList<string> Data { get => data; set => data = value; }

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


        public ScatterChart(IList<string> variableList)
        {
            Data = variableList;
        }

        public void Update()
        {
            ggplot = new Ggplot();

            var newLayer = new Layer("point");

            var aestheticXml = Properties.Resources.ResourceManager.GetObject("geom_point");
            var aesthetic = xmlConverter.ToObject<Aesthetic>(aestheticXml.ToString());
            newLayer.Aes = aesthetic;

            ggplot.Layers.Add(newLayer);
        }

        public bool CanPlot()
        {
            return data.Count == 2;
             //   && 
            //    (((IVariable)data[0]).Length == ((IVariable)data[1]).Length) &&
            //    ((IVariable)data[0]).Data== DataType.Numeric &&
            //    ((IVariable)data[1]).Data == DataType.Numeric;
        }
    }
}
