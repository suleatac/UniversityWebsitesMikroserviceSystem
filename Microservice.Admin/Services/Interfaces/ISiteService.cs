using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Site;

namespace Microservice.Admin.Services.Interfaces
{
    public interface ISiteService
    {
        Task<ServiceResult<List<SiteGetVm>>> GetSitesAsync();
        Task<ServiceResult<SiteGetVm>> GetSiteByIdAsync(int id);
        Task<ServiceResult<SiteGetVm>> CreateSiteAsync(CreateSiteVm dto);
        Task<ServiceResult<bool>> UpdateSiteAsync(UpdateSiteVm dto);
        Task<ServiceResult<bool>> DeleteSiteAsync(int id);
    }
}
