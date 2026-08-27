namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface ISiteRepository : IGenericRepository<Mikroservice.Site.Domain.Entities.Site>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<Mikroservice.Site.Domain.Entities.Site?> GetSiteByHostAsync(string host, CancellationToken cancellationToken = default);
    }
}
