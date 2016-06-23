﻿using System;
using System.Collections.Generic;
using EnjoyCQRS.Events;
using EnjoyCQRS.EventSource;

namespace EnjoyCQRS.MetadataProviders
{
    public class CorrelationIdMetadataProvider : IMetadataProvider
    {
        private Guid _correlationId;

        public CorrelationIdMetadataProvider()
        {
            _correlationId = Guid.NewGuid();
        }

        public IEnumerable<KeyValuePair<string, string>> Provide<TAggregate>(TAggregate aggregate, IDomainEvent @event, IMetadata metadata) where TAggregate : IAggregate
        {
            yield return new KeyValuePair<string, string>(MetadataKeys.CorrelationId, _correlationId.ToString());
        }
    }
}