﻿using Prism.Logging;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using TNCodeApp.Docking;

namespace TNCodeApp.Logger
{
    public class LoggerViewModel : BindableBase, ITnPanel, ILoggerFacade
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