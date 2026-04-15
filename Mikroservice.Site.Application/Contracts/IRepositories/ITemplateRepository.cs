using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface ITemplateRepository : IGenericRepository<Template>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
