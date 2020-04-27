using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TnCode.Core.Data;
using TnCode.TnCodeApp.Data.Events;
using TnCode.TnCodeApp.Progress;

namespace TnCode.TnCodeApp.Data.ViewModels
{
    public class DataRibbonViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDataSetsManager dataSetsManager;
        private readonly IRManager rManager;
        private IProgressService progressService;
        private IDialogService dialogService;

        public DelegateCommand LoadRCommand { get; private set; }
        public DelegateCommand LoadCsvCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        private bool isRRunning;

        public bool IsRRunning
        {
            get => rManager.IsRRunning;
            set
            {
                SetProperty(ref isRRunning, value);
            }
        }

        public DataRibbonViewModel(IEventAggregator eventAgg, IDataSetsManager dataMgr, IRManager rMgr, IProgressService pService, IDialogService dService)
        {
            eventAggregator = eventAgg;
            dataSetsManager = dataMgr;
            rManager = rMgr;
            progressService = pService;
            dialogService = dService;

            LoadRCommand = new DelegateCommand(LoadRData).ObservesCanExecute(() => IsRRunning);
            LoadCsvCommand = new DelegateCommand(LoadCsvData).ObservesCanExecute(() => IsRRunning); ;
            SaveCommand = new DelegateCommand(Save).ObservesCanExecute(() => IsRRunning);

            rManager.RConnected += RManager_ConnectionChanged;
            rManager.RDisconnected += RManager_ConnectionChanged;
        }


        private void RManager_ConnectionChanged(object sender, EventArgs e)
        {
            IsRRunning = rManager.IsRRunning;
        }

        private void Save()
        {
            throw new NotImplementedException();
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
        }

        private async void LoadRData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "R Workspace (*.RData)|*.RData";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                var fileFullPath = openFileDialog.FileName;

                await progressService.ExecuteAsync(rManager.LoadRWorkSpace, fileFullPath);
            }
        }
    }
}