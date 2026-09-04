using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.Enums;

namespace Microservice.Site.Persistence.Repositories
{
    public class PageTypeRepository
        : GenericRepository<PageType>, IPageTypeRepository
    {
        private readonly AppDbContext _appDbContext;

        public PageTypeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<PageType?> GetPageTypeByTemplateIdAndPageTypeKind(int templateId, PageTypeKind pageTypeKind, int dilId, CancellationToken cancellationToken = default)
        {
            return _appDbContext.PageTypes
            .FirstOrDefaultAsync(x => x.TemplateId == templateId && x.PageTypeKind == pageTypeKind && x.DilId == dilId, cancellationToken);
        }


    }
}