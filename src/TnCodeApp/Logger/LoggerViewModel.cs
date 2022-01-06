using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using TnCode.TnCodeApp.Docking;

namespace TnCode.TnCodeApp.Logger
{
    public class LoggerViewModel : BindableBase, IDockingPanel, ILogger
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

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var newEntry = new LogEntry()
            {
                Category = logLevel.ToString(),
                Message = formatter(state, exception),
            };
            LogEntries.Insert(0, newEntry);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}
