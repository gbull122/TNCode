using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using TNCode.Core.Data;
using TNCodeApp.Chart.Events;
using TNCodeApp.Progress;
using TNCodeApp.R.Views;

namespace TNCodeApp.Chart
{
    public class ChartManager : IChartManager
    {
        private readonly Dictionary<string, Type> dict = new Dictionary<string, Type>();

        private readonly Dictionary<string, IChart> charts = new Dictionary<string, IChart>();

        private string lastChart = string.Empty;

        private readonly IEventAggregator eventAggregator;

        private readonly IXmlConverter xmlConverter;

        private readonly IRegionManager regionManager;


        public ChartManager(IEventAggregator eventAgg, IXmlConverter xCon, IRegionManager regionMgr)
        {
            eventAggregator = eventAgg;
            xmlConverter = xCon;
            regionManager = regionMgr;

            Register<ScatterChart>("scatter");
            Register<LineChart>("line");
        }


        public IChart GetChart(string key)
        {
            return charts[key];
        }

        public IChart GetLastChart()
        {
            return charts[lastChart];
        }

        public void Create(string id, params object[] args)
        {
            ///TODO should return chart and viewmodel should be naviagate
            Type type = null;
            if (dict.TryGetValue(id, out type))
            {
                var newChart =  (IChart)Activator.CreateInstance(type, args);
                newChart.Converter = xmlConverter;

                if (newChart.CanPlot())
                {
                    newChart.Update();
                    newChart.Title = "chart " + charts.Keys.Count + 1;
                    charts.Add(newChart.Title, newChart);
                    lastChart = newChart.Title;

                    var naviParameters = new NavigationParameters();
                    naviParameters.Add("chart", newChart);

                    regionManager.RequestNavigate("MainRegion", new Uri("ChartBuilderView", UriKind.Relative),naviParameters);

                    return;
                }

                eventAggregator.GetEvent<DisplayMessageEvent>().Publish("Can't create chart with selected variables.");
            }

            
        }

        public void Register<Tderived>(string id) where Tderived : IChart
        {
            var type = typeof(Tderived);

            if (type.IsInterface || type.IsAbstract)
                throw new ArgumentException("...");

            dict.Add(id, type);
        }


    }
}
