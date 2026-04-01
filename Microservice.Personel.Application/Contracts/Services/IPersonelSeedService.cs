using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Personel.Application.Contracts.Services
{
    public interface IPersonelSeedService
    {
        Task SeedInitialDataAsync(CancellationToken cancellationToken = default);
        Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default);
    }
}
