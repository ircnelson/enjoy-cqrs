using Microsoft.Extensions.Logging;

namespace Cars.Logger
{
    public class NoopLoggerFactory : ILoggerFactory
    {

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new NoopLogger(categoryName);
        }

        public void AddProvider(ILoggerProvider provider)
        {
            //Don't do anything...it's a noop
        }
    }
}