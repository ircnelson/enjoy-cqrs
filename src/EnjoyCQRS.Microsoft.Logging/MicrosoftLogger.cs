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

        public void Log(MsLogging.LogLevel logLevel, string message, Exception exception = null)
        {
            switch (logLevel)
            {
                case MsLogging.LogLevel.Critical:
                    MsLogging.LoggerExtensions.LogCritical(_logger, new MsLogging.EventId(), exception, message);
                    break;
                case MsLogging.LogLevel.Debug:
                    MsLogging.LoggerExtensions.LogDebug(_logger, new MsLogging.EventId(), exception, message);
                    break;
                case MsLogging.LogLevel.Error:
                    MsLogging.LoggerExtensions.LogError(_logger, new MsLogging.EventId(), exception, message);
                    break;
                case MsLogging.LogLevel.Information:
                    MsLogging.LoggerExtensions.LogInformation(_logger, new MsLogging.EventId(), exception, message);
                    break;
                case MsLogging.LogLevel.None:
                    break;
                case MsLogging.LogLevel.Trace:
                    MsLogging.LoggerExtensions.LogTrace(_logger, new MsLogging.EventId(), exception, message);
                    break;
                case MsLogging.LogLevel.Warning:
                    MsLogging.LoggerExtensions.LogWarning(_logger, new MsLogging.EventId(), exception, message);
                    break;
                default:
                    throw new InvalidOperationException("The requested log level is not supported.");
            }
        }

        public bool IsEnabled(MsLogging.LogLevel logLevel)
        {
            return _logger.IsEnabled(ToMicrosoftLogLevel(logLevel));
        }

        private static MsLogging.LogLevel ToMicrosoftLogLevel(MsLogging.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case MsLogging.LogLevel.Critical:
                    return MsLogging.LogLevel.Critical;
                case MsLogging.LogLevel.Debug:
                    return MsLogging.LogLevel.Debug;
                case MsLogging.LogLevel.Error:
                    return MsLogging.LogLevel.Error;
                case MsLogging.LogLevel.Information:
                    return MsLogging.LogLevel.Information;
                case MsLogging.LogLevel.None:
                    return MsLogging.LogLevel.None;
                case MsLogging.LogLevel.Trace:
                    return MsLogging.LogLevel.Trace;
                case MsLogging.LogLevel.Warning:
                    return MsLogging.LogLevel.Warning;
                default:
                    throw new InvalidOperationException("The requested log level is not supported.");
            }
        }

    }
}
