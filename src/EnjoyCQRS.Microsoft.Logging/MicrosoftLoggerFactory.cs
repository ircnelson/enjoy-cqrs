using EnjoyCQRS.Logger;
using MsLogging = Microsoft.Extensions.Logging;

namespace EnjoyCQRS.Microsoft.Logging
{
    public class MicrosoftLoggerFactory : MsLogging.ILoggerFactory
    {
        private readonly MsLogging.ILoggerFactory _loggerFactory;

        public MicrosoftLoggerFactory(MsLogging.ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public ILogger Create(string name)
        {
            return new MicrosoftLogger(_loggerFactory.CreateLogger(name));
        }

        public void Dispose()
        {
        }
    }
}
