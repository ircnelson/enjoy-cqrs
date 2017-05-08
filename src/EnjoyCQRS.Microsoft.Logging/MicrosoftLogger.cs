using System;
using EnjoyCQRS.Logger;
using MsLogging = Microsoft.Extensions.Logging;

namespace EnjoyCQRS.Microsoft.Logging
{
    public class MicrosoftLogger : ILogger
    {
        private readonly MsLogging.ILogger _logger;

        public MicrosoftLogger(MsLogging.ILogger logger)
        {
            _logger = logger;
        }

        public void Log(LogLevel logLevel, string message, Exception exception = null)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    MsLogging.LoggerExtensions.LogCritical(_logger, new MsLogging.EventId(), exception, message);
                    break;
                case LogLevel.Debug:
                    MsLogging.LoggerExtensions.LogDebug(_logger, new MsLogging.EventId(), exception, message);
                    break;
                case LogLevel.Error:
                    MsLogging.LoggerExtensions.LogError(_logger, new MsLogging.EventId(), exception, message);
                    break;
                case LogLevel.Information:
                    MsLogging.LoggerExtensions.LogInformation(_logger, new MsLogging.EventId(), exception, message);
                    break;
                case LogLevel.None:
                    break;
                case LogLevel.Trace:
                    MsLogging.LoggerExtensions.LogTrace(_logger, new MsLogging.EventId(), exception, message);
                    break;
                case LogLevel.Warning:
                    MsLogging.LoggerExtensions.LogWarning(_logger, new MsLogging.EventId(), exception, message);
                    break;
                default:
                    throw new InvalidOperationException("The requested log level is not supported.");
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(ToMicrosoftLogLevel(logLevel));
        }

        private static MsLogging.LogLevel ToMicrosoftLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return MsLogging.LogLevel.Critical;
                case LogLevel.Debug:
                    return MsLogging.LogLevel.Debug;
                case LogLevel.Error:
                    return MsLogging.LogLevel.Error;
                case LogLevel.Information:
                    return MsLogging.LogLevel.Information;
                case LogLevel.None:
                    return MsLogging.LogLevel.None;
                case LogLevel.Trace:
                    return MsLogging.LogLevel.Trace;
                case LogLevel.Warning:
                    return MsLogging.LogLevel.Warning;
                default:
                    throw new InvalidOperationException("The requested log level is not supported.");
            }
        }

    }
}
