using System.Collections.Generic;
using System.Linq;
using Cars.Testing.Shared.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Cars.UnitTests.Logging
{
    public class WhenLogging
    {
        private readonly IList<TestMessage> _messages = new List<TestMessage>();
        private readonly ILoggerFactory _loggerFactory;

        public WhenLogging()
        {
            _loggerFactory = new TestLoggerFactory(true, _messages);
        }

        [Fact]
        public void Can_log_critical_message()
        {
            var logger = _loggerFactory.CreateLogger("TestLogger");
             
            logger.LogCritical("Critical Message");

            var loggedMessage = _messages.LastOrDefault();
            AssertionExtensions.Should((object) loggedMessage).NotBeNull();
            AssertionExtensions.Should((string) loggedMessage.LogLevel.ToString()).Be("Critical");
            AssertionExtensions.Should((object) loggedMessage.Exception).BeNull();
            AssertionExtensions.Should((string) loggedMessage.LoggerName).Be("TestLogger");
            AssertionExtensions.Should((string) loggedMessage.Message).Be("Critical Message");
        }

        [Fact]
        public void Can_log_error_message()
        {
            var logger = _loggerFactory.CreateLogger("TestLogger");

            logger.LogError("Error Message");

            var loggedMessage = _messages.LastOrDefault();
            AssertionExtensions.Should((object) loggedMessage).NotBeNull();
            AssertionExtensions.Should((string) loggedMessage.LogLevel.ToString()).Be("Error");
            AssertionExtensions.Should((object) loggedMessage.Exception).BeNull();
            AssertionExtensions.Should((string) loggedMessage.LoggerName).Be("TestLogger");
            AssertionExtensions.Should((string) loggedMessage.Message).Be("Error Message");
        }

        [Fact]
        public void Can_log_warning_message()
        {
            var logger = _loggerFactory.CreateLogger("TestLogger");

            logger.LogWarning("Warning Message");

            var loggedMessage = _messages.LastOrDefault();
            AssertionExtensions.Should((object) loggedMessage).NotBeNull();
            AssertionExtensions.Should((string) loggedMessage.LogLevel.ToString()).Be("Warning");
            AssertionExtensions.Should((object) loggedMessage.Exception).BeNull();
            AssertionExtensions.Should((string) loggedMessage.LoggerName).Be("TestLogger");
            AssertionExtensions.Should((string) loggedMessage.Message).Be("Warning Message");
        }

        [Fact]
        public void Can_log_information_message()
        {
            var logger = _loggerFactory.CreateLogger("TestLogger");

            logger.LogInformation("Information Message");

            var loggedMessage = _messages.LastOrDefault();
            AssertionExtensions.Should((object) loggedMessage).NotBeNull();
            AssertionExtensions.Should((string) loggedMessage.LogLevel.ToString()).Be("Information");
            AssertionExtensions.Should((object) loggedMessage.Exception).BeNull();
            AssertionExtensions.Should((string) loggedMessage.LoggerName).Be("TestLogger");
            AssertionExtensions.Should((string) loggedMessage.Message).Be("Information Message");
        }

        [Fact]
        public void Can_log_debug_message()
        {
            var logger = _loggerFactory.CreateLogger("TestLogger");

            logger.LogDebug("Debug Message");

            var loggedMessage = _messages.LastOrDefault();
            AssertionExtensions.Should((object) loggedMessage).NotBeNull();
            AssertionExtensions.Should((string) loggedMessage.LogLevel.ToString()).Be("Debug");
            AssertionExtensions.Should((object) loggedMessage.Exception).BeNull();
            AssertionExtensions.Should((string) loggedMessage.LoggerName).Be("TestLogger");
            AssertionExtensions.Should((string) loggedMessage.Message).Be("Debug Message");
        }

        [Fact]
        public void Can_log_trace_message()
        {
            var logger = _loggerFactory.CreateLogger("TestLogger");

            logger.LogTrace("Trace Message");

            var loggedMessage = _messages.LastOrDefault();
            AssertionExtensions.Should((object) loggedMessage).NotBeNull();
            AssertionExtensions.Should((string) loggedMessage.LogLevel.ToString()).Be("Trace");
            AssertionExtensions.Should((object) loggedMessage.Exception).BeNull();
            AssertionExtensions.Should((string) loggedMessage.LoggerName).Be("TestLogger");
            AssertionExtensions.Should((string) loggedMessage.Message).Be("Trace Message");
        }

        [Fact]
        public void Disabled_logger_doesnt_log_a_message()
        {
            _messages.Clear();
            var loggerFactory = new TestLoggerFactory(false, _messages);
            var logger = loggerFactory.CreateLogger("DisabledLogger");

            LoggerExtensions.LogCritical(logger, "Disabled logger message");

            var loggedMessage = _messages.LastOrDefault();
            AssertionExtensions.Should((object) loggedMessage).BeNull();
        }
    }

}