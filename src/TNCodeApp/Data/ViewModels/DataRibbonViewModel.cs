using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.IO;
using TNCode.Core.Data;
using TNCodeApp.Data.Events;
using TNCodeApp.Progress;
using TNCodeApp.R;
using System.Linq;
using System.Threading.Tasks;

namespace TNCodeApp.Data.ViewModels
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

                var task = new Task(() => LoadCsvFile(openFileDialog.FileName, dataSetName));

                await progressService.ExecuteAsync(task, "Loading CSV file ");
            }
        }

        private void LoadCsvFile(string path, string datasetName)
        {
            var rowWiseRawData = dataSetsManager.ReadCsvFileRowWise(path);
            var colWiseData = dataSetsManager.RowWiseToColumnWise(rowWiseRawData);
            var newDataSet = new DataSet(colWiseData, datasetName);

            
            var dataSetEventArgs = new DataSetEventArgs();
            dataSetEventArgs.Modification = DataSetChange.Added;
            dataSetEventArgs.Data = newDataSet;
            eventAggregator.GetEvent<DataSetChangedEvent>().Publish(dataSetEventArgs);
        }

        private async void LoadRData()
        {
            ///TODO move into model
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "R Workspace (*.RData)|*.RData";
            if (openFileDialog.ShowDialog() == true)
            {
               var fileFullPath = openFileDialog.FileName;

               await progressService.ExecuteAsync(rManager.LoadRWorkSpace, fileFullPath);
               //await rManager.LoadRWorkSpace(fileFullPath, progress);
            }
        }

        
    }
}
