using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.Enums;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IPageTypeRepository : IGenericRepository<PageType>
    {
        Task<PageType?> GetPageTypeByTemplateIdAndPageTypeKind(int templateId, PageTypeKind pageTypeKind, int dilId, CancellationToken cancellationToken = default);
    }
}