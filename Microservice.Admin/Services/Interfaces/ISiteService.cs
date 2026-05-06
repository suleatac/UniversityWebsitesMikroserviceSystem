using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Site;

namespace Microservice.Admin.Services.Interfaces
{
    public interface ISiteService
    {
        Task<ServiceResult<List<SiteGetVm>>> GetSitesAsync();
        Task<ServiceResult<SiteDetailGetVm>> GetSiteByIdAsync(int id);
        Task<ServiceResult<object>> CreateSiteAsync(CreateSiteVm dto);
        Task<ServiceResult<bool>> UpdateSiteAsync(SiteDetailGetVm dto);
        Task<ServiceResult<bool>> DeleteSiteAsync(int id);
        Task<ServiceResult<PaginatedResult<SiteGetVm>>> GetSitesPaginatedAsync(
            int page, int pageSize, string? search, string? orderBy, string? orderDir);
    }
}
