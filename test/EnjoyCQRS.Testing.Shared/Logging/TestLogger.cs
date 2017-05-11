// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;

namespace Cars.Testing.Shared.Logging
{
    public class TestLogger : ILogger
    {
        private readonly string _name;
        private readonly bool _enabled;
        private readonly IList<TestMessage> _messages;


        public TestLogger(string name, bool enabled, IList<TestMessage> messages)
        {
            _name = name;
            _enabled = enabled;
            _messages = messages;
        }

        public string Name { get; set; }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            _messages.Add(new TestMessage
            {
                LogLevel = logLevel,
                EventId = eventId,
                State = state,
                Exception = exception,
                Message = formatter(state, exception),
                LoggerName = _name,
            });
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _enabled;
        }

        private class TestDisposable : IDisposable
        {
            public static readonly TestDisposable Instance = new TestDisposable();

            public void Dispose()
            {
                // intentionally does nothing
            }
        }
    }
}