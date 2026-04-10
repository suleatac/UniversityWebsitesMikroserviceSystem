using Microservice.Yonetici.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Yonetici.Application.Contracts.IRepositories
{
    public interface IYoneticiTipiRepository : IGenericRepository<YoneticiTipi>
    {
        Task<YoneticiTipi?> GetYoneticiTipiByValue(int value, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
