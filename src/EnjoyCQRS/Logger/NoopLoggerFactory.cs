namespace EnjoyCQRS.Logger
{
    public class NoopLoggerFactory : ILoggerFactory
    {

        public void Dispose()
        {
        }

        public ILogger Create(string name)
        {
            return new NoopLogger(name);
        }
    }
}