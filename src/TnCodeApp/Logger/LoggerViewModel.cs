using Prism.Logging;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using TnCode.TnCodeApp.Docking;

namespace TnCode.TnCodeApp.Logger
{
    public class LoggerViewModel : BindableBase, IDockingPanel, ILoggerFacade
    {

        private ObservableCollection<LogEntry> logEntries;

        public ObservableCollection<LogEntry> LogEntries
        {
            get { return logEntries; }
            set
            {
                logEntries = value;
                RaisePropertyChanged(nameof(logEntries));
            }
        }

        public DockingMethod Docking { get => DockingMethod.StatusPanel; }

        public string Title { get => "Log"; }

        public LoggerViewModel()
        {
            logEntries = new ObservableCollection<LogEntry>();
        }

        public void Log(string message, Category category, Priority priority)
        {
            var newEntry = new LogEntry()
            {
                Category = category.ToString(),
                Message = message
            };
            LogEntries.Add(newEntry);
        }
    }
}
