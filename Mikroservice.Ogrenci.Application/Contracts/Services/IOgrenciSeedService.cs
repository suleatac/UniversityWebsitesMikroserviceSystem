using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Ogrenci.Application.Contracts.Services
{
    public interface IOgrenciSeedService
    {
        Task SeedInitialDataAsync(CancellationToken cancellationToken = default);
        Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default);
    }
}
