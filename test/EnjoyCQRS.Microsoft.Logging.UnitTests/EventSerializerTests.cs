using System.Collections.Generic;
using System.Linq;
using EnjoyCQRS.Logger;
using EnjoyCQRS.Microsoft.Logging.UnitTests.Stubs;
using FluentAssertions;
using Xunit;

namespace EnjoyCQRS.Microsoft.Logging.UnitTests
{
    public class WhenLoggingWithMicrosoftLogger
    {
        private readonly IList<TestMessage> _messages = new List<TestMessage>();
        private readonly ILoggerFactory _loggerFactory;

        public WhenLoggingWithMicrosoftLogger()
        {
            _loggerFactory = new MicrosoftLoggerFactory(new TestLoggerFactory(true, _messages));
        }

        [Fact]
        public void Can_log_critical_message()
        {
            var logger = _loggerFactory.Create("TestLogger");
             
            logger.Log(LogLevel.Critical, "Critical Message");

            var loggedMessage = _messages.LastOrDefault();
            loggedMessage.Should().NotBeNull();
            loggedMessage.LogLevel.ToString().Should().Be("Critical");
            loggedMessage.Exception.Should().BeNull();
            loggedMessage.LoggerName.Should().Be("TestLogger");
            loggedMessage.Message.Should().Be("Critical Message");
        }

        [Fact]
        public void Can_log_error_message()
        {
            var logger = _loggerFactory.Create("TestLogger");

            logger.Log(LogLevel.Error, "Error Message");

            var loggedMessage = _messages.LastOrDefault();
            loggedMessage.Should().NotBeNull();
            loggedMessage.LogLevel.ToString().Should().Be("Error");
            loggedMessage.Exception.Should().BeNull();
            loggedMessage.LoggerName.Should().Be("TestLogger");
            loggedMessage.Message.Should().Be("Error Message");
        }

        [Fact]
        public void Can_log_warning_message()
        {
            var logger = _loggerFactory.Create("TestLogger");

            logger.Log(LogLevel.Warning, "Warning Message");

            var loggedMessage = _messages.LastOrDefault();
            loggedMessage.Should().NotBeNull();
            loggedMessage.LogLevel.ToString().Should().Be("Warning");
            loggedMessage.Exception.Should().BeNull();
            loggedMessage.LoggerName.Should().Be("TestLogger");
            loggedMessage.Message.Should().Be("Warning Message");
        }

        [Fact]
        public void Can_log_information_message()
        {
            var logger = _loggerFactory.Create("TestLogger");

            logger.Log(LogLevel.Information, "Information Message");

            var loggedMessage = _messages.LastOrDefault();
            loggedMessage.Should().NotBeNull();
            loggedMessage.LogLevel.ToString().Should().Be("Information");
            loggedMessage.Exception.Should().BeNull();
            loggedMessage.LoggerName.Should().Be("TestLogger");
            loggedMessage.Message.Should().Be("Information Message");
        }

        [Fact]
        public void Can_log_debug_message()
        {
            var logger = _loggerFactory.Create("TestLogger");

            logger.Log(LogLevel.Debug, "Debug Message");

            var loggedMessage = _messages.LastOrDefault();
            loggedMessage.Should().NotBeNull();
            loggedMessage.LogLevel.ToString().Should().Be("Debug");
            loggedMessage.Exception.Should().BeNull();
            loggedMessage.LoggerName.Should().Be("TestLogger");
            loggedMessage.Message.Should().Be("Debug Message");
        }

        [Fact]
        public void Can_log_trace_message()
        {
            var logger = _loggerFactory.Create("TestLogger");

            logger.Log(LogLevel.Trace, "Trace Message");

            var loggedMessage = _messages.LastOrDefault();
            loggedMessage.Should().NotBeNull();
            loggedMessage.LogLevel.ToString().Should().Be("Trace");
            loggedMessage.Exception.Should().BeNull();
            loggedMessage.LoggerName.Should().Be("TestLogger");
            loggedMessage.Message.Should().Be("Trace Message");
        }

        [Fact]
        public void Logging_none_level_doesnt_log_a_message()
        {
            _messages.Clear();
            var logger = _loggerFactory.Create("TestLogger");

            logger.Log(LogLevel.None, "None Message");

            var loggedMessage = _messages.LastOrDefault();
            loggedMessage.Should().BeNull();
        }

        [Fact]
        public void Disabled_logger_doesnt_log_a_message()
        {
            _messages.Clear();
            var loggerFactory = new MicrosoftLoggerFactory(new TestLoggerFactory(false, _messages));
            var logger = loggerFactory.Create("DisabledLogger");

            logger.Log(LogLevel.Critical, "Disabled logger message");

            var loggedMessage = _messages.LastOrDefault();
            loggedMessage.Should().BeNull();
        }
    }

}