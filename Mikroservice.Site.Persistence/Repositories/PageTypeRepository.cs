using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Persistence.Repositories
{
    public class PageTypeRepository(AppDbContext appDbContext)
        : GenericRepository<PageType>(appDbContext), IPageTypeRepository
    {
    }
}