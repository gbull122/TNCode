﻿using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.IO;
using System.Threading.Tasks;
using TnCode.TnCodeApp.Data.Events;
using TnCode.TnCodeApp.Progress;

namespace TnCode.TnCodeApp.Data.ViewModels
{
    public class DataRibbonViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDataSetsManager dataSetsManager;
        private IProgressService progressService;
        private IDialogService dialogService;


        public DelegateCommand LoadCsvCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public DataRibbonViewModel(IEventAggregator eventAgg, IDataSetsManager dataMgr, IProgressService pService, IDialogService dService)
        {
            eventAggregator = eventAgg;
            dataSetsManager = dataMgr;
            progressService = pService;
            dialogService = dService;


            LoadCsvCommand = new DelegateCommand(LoadCsvData); ;
            SaveCommand = new DelegateCommand(Save, CanSave);
        }

        private bool CanSave()
        {
            if (dataSetsManager.DataSets.Count > 0)
                return true;

            return false;
        }

        private void Save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Directory | directory";
            saveFileDialog.FileName = "Save here";
            if (saveFileDialog.ShowDialog() == true)
            {
                string destinationPath = Path.GetDirectoryName(saveFileDialog.FileName);

                dataSetsManager.SaveAllDataSetsToCsv(destinationPath);
            }
        }

        private async void LoadCsvData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Csv files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                var dataSetName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);

                if (dataSetsManager.DatasetNameExists(dataSetName))
                {
                    var message = "This is a message that should be shown in the dialog.";
                    //using the dialog service as-is
                    dialogService.ShowDialog("ConfirmationDialogView", new DialogParameters($"message={message}"), r =>
                    {

                        if (r.Result == ButtonResult.None)
                            return;
                        //Title = "Result is None";
                        else if (r.Result == ButtonResult.OK)
                            return;
                        //Title = "Result is OK";
                        else if (r.Result == ButtonResult.Cancel)
                            return;
                        //Title = "Result is Cancel";
                        else
                            //Title = "I Don't know what you did!?";
                            return;
                    });
                    return;
                }

                await progressService.ExecuteAsync(LoadCsvFile, openFileDialog.FileName, dataSetName);
            }
        }

        private async Task LoadCsvFile(IProgress<string> progress, string path, string datasetName)
        {
            progress.Report("Reading File...");
            var rowWiseRawData = dataSetsManager.ReadCsvFileRowWise(path);
            var colWiseData = dataSetsManager.RowWiseToColumnWise(rowWiseRawData);

            progress.Report("Importing Data...");
            var newDataSet = new DataSet(colWiseData, datasetName);

            var dataSetEventArgs = new DataSetEventArgs();
            dataSetEventArgs.Modification = DataSetChange.Added;
            dataSetEventArgs.Data = newDataSet;
            eventAggregator.GetEvent<DataSetChangedEvent>().Publish(dataSetEventArgs);

            SaveCommand.RaiseCanExecuteChanged();
        }


    }
}