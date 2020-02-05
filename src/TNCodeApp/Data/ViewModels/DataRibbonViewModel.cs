using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.IO;
using TNCode.Core.Data;
using TNCodeApp.Data.Events;
using TNCodeApp.Progress;
using TNCodeApp.R;

namespace TNCodeApp.Data.ViewModels
{
    public class DataRibbonViewModel:BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDataSetsManager dataSetsManager;
        private readonly IRManager rManager;
        private IProgressService progressService;

        public DelegateCommand LoadRCommand { get; private set; }
        public DelegateCommand LoadCsvCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public DataRibbonViewModel(IEventAggregator eventAgg, IDataSetsManager dataMgr, IRManager rMgr, IProgressService pService)
        {
            eventAggregator = eventAgg;
            dataSetsManager = dataMgr;
            rManager = rMgr;
            progressService = pService;

            LoadRCommand = new DelegateCommand(LoadRData);
            LoadCsvCommand = new DelegateCommand(LoadCsvData);
            SaveCommand = new DelegateCommand(Save);
        }

        private void Save()
        {
            throw new NotImplementedException();
        }

        private void LoadCsvData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Csv files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                var dataSetName = Path.GetFileName(openFileDialog.FileName);

                if (dataSetsManager.DatasetNameExists(dataSetName))
                    return;

                var rawData = dataSetsManager.ReadCsvFile(openFileDialog.FileName);
                var newDataSet = new DataSet(rawData, dataSetName);

                eventAggregator.GetEvent<NewDataSetEvent>().Publish(newDataSet);
            }
        }

        private async void LoadRData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "R Workspace (*.RData)|*.RData";
            if (openFileDialog.ShowDialog() == true)
            {
                var fileFullPath = openFileDialog.FileName;
                await rManager.LoadToTempEnv(fileFullPath);

                var things = await rManager.TempEnvObjects();
                foreach(string thing in things)
                {
                    var isDataFrame = await rManager.IsDataFrame(thing);
                    if (isDataFrame)
                    {
                        //await progressService.ExecuteAsync(rManager.InitialiseAsync(), "Starting R...");
                        var importedData = await rManager.GetDataFrameAsDataSetAsync(thing);
                        eventAggregator.GetEvent<NewDataSetEvent>().Publish(importedData);
                    }

                }
            }
        }
    }
}
