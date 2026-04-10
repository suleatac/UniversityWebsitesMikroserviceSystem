using Microservice.Yonetici.Domain.Entities;

namespace Microservice.Yonetici.Application.Contracts.IRepositories
{
    public interface IYoneticiDuyuruRepository : IGenericRepository<YoneticiDuyuru>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
