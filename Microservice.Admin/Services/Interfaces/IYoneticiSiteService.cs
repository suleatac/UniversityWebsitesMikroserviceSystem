using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.YoneticiSite;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IYoneticiSiteService
    {
        Task<ServiceResult<List<GetYoneticiSiteVm>>> GetYoneticiSitesAsync(int siteId);
        Task<ServiceResult<YoneticiSiteDetailVm>> GetYoneticiSiteByIdAsync(int id);
        Task<ServiceResult<object>> CreateYoneticiSiteAsync(CreateYoneticiSiteVm dto);
        Task<ServiceResult<object>> UpdateYoneticiSiteAsync(YoneticiSiteDetailVm dto);
        Task<ServiceResult<object>> DeleteYoneticiSiteAsync(int id);
    }
}