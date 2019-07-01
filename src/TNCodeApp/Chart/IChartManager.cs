namespace TNCodeApp.Chart
{
    public interface IChartManager
    {
        void Create(string id, params object[] args);

        IChart GetChart(string key);

        IChart GetLastChart();
    }
}