using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.IO;
using TNCode.Core.Data;
using TNCodeApp.Data.Events;

namespace TNCodeApp.Data.ViewModels
{
    public class DataRibbonViewModel:BindableBase
    {

        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public DelegateCommand<string> DataCommand { get; private set; }

        public DelegateCommand LoadCommand { get; private set; }

        public bool IsMainRibbon => true;

        public DataRibbonViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            NavigateCommand = new DelegateCommand<string>(Navigate);
            DataCommand = new DelegateCommand<string>(Data);
            LoadCommand = new DelegateCommand(LoadData);
        }

        private void LoadData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Csv files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                var rawData = ReadCsvFile(openFileDialog.FileName);

                var newDataSet = new DataSet(rawData, Path.GetFileName(openFileDialog.FileName));

                eventAggregator.GetEvent<NewDataSetEvent>().Publish(newDataSet);
            }
        }

        public List<string[]> ReadCsvFile(string filePath)
        {
            var reader = new StreamReader(File.OpenRead(filePath));
            List<string[]> rawData = new List<string[]>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                rawData.Add(line.Split(','));
            }

            return rawData;
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                regionManager.RequestNavigate("MainRegion", navigatePath);
        }

        private void Data(string action)
        {
            eventAggregator.GetEvent<TestDataEvent>().Publish("Mpg");
        }
    }
}
