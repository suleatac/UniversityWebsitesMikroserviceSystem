using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IPopupRepository : IGenericRepository<Popup>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
