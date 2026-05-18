using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Contracts.IRepositories
{
    public interface IAuditLogRepository : IGenericRepository<AuditLog>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
