using Prism.Events;
using System;
using System.Collections.Generic;
using TNCodeApp.Chart.Events;
using TNCodeApp.Progress;

namespace TNCodeApp.Chart
{
    public class ChartManager : IChartManager
    {
        private Dictionary<string, Type> _dict = new Dictionary<string, Type>();

        private Dictionary<string, IChart> charts = new Dictionary<string, IChart>();

        private string lastChart = string.Empty;

        private IEventAggregator eventAggregator;

        public ChartManager(IEventAggregator eventAgg)
        {
            eventAggregator = eventAgg;
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
            Type type = null;
            if (_dict.TryGetValue(id, out type))
            {
                var newChart =  (IChart)Activator.CreateInstance(type, args);

                if (newChart.CanPlot())
                {
                    newChart.Title = "chart " + charts.Keys.Count + 1;
                    charts.Add(newChart.Title, newChart);
                    lastChart = newChart.Title;

                    eventAggregator.GetEvent<ChartCreatedEvent>().Publish(newChart.Title);
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

            _dict.Add(id, type);
        }


    }
}
