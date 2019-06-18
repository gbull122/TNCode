using System;
using Prism.Commands;
using Prism.Mvvm;

namespace TNCodeApp.Chart.ViewModels
{
    public class ChartRibbonViewModel:BindableBase
    {
        public DelegateCommand<string> ChartCommand { get; private set; }

        public ChartRibbonViewModel()
        {

            ChartCommand = new DelegateCommand<string>(GenerateChart);
        }

        private void GenerateChart(string chartName)
        {
            throw new NotImplementedException();
        }
    }
}
