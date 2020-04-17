using NLog;
using Prism.Logging;
using System;

namespace TNCodeApp.Logger
{

    public class TnLogger : ILoggerFacade
    {
        private NLog.Logger logger;

        public TnLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "TnLog.txt" };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;

            logger = LogManager.GetCurrentClassLogger();

            logger.Log(LogLevel.Info, "App started");
        }

        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    logger.Debug(message);
                    break;
                case Category.Warn:
                    logger.Warn(message);
                    break;
                case Category.Exception:
                    logger.Error(message);
                    break;
                case Category.Info:
                    logger.Info(message);
                    break;
            }
        }
    }
}
