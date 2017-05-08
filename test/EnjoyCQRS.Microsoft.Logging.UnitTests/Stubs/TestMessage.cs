using System;
using Microsoft.Extensions.Logging;

namespace EnjoyCQRS.Microsoft.Logging.UnitTests.Stubs
{
    public class TestMessage
    {
        public LogLevel LogLevel { get; set; }

        public EventId EventId { get; set; }

        public object State { get; set; }

        public Exception Exception { get; set; }

        public String Message { get; set; }

        public string LoggerName { get; set; }
    }
}
