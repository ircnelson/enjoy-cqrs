﻿using System;
using System.Threading.Tasks;

namespace Cars.EventSource.Projections
{
    public interface IProjectionProviderScanner
    {
        Task<ScannerResult> ScanAsync(Type type);
        Task<ScannerResult> ScanAsync<TAggregate>() where TAggregate : IAggregate;
    }
}