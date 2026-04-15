using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
