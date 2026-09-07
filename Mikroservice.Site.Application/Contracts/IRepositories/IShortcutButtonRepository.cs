using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Contracts.IRepositories
{
    public interface IShortcutButtonRepository : IGenericRepository<ShortcutButton>
    {
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<List<ShortcutButton>> GetShortcutButtonsBySiteAndDilAsync(int siteId, int dilId, CancellationToken cancellationToken = default);
    }
    
}
