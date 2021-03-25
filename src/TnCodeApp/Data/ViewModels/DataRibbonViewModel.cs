using Microsoft.Win32;
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
                    var dialogParameters = new DialogParameters()
                    {
                        { "title", "Duplicate Data" },
                         { "message", string.Format("There is already a DataSet called {0}. Rename the csv file and try again.",dataSetName) }
                    };

                    void Callback(IDialogResult result)
                    {
                        
                    }

                    dialogService.ShowDialog("NotificationDialogView", dialogParameters, Callback);
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