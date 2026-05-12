using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.YoneticiSite;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IYoneticiSiteService
    {
        Task<ServiceResult<List<YoneticiSiteDetailVm>>> GetYoneticiSitesAsync();
        Task<ServiceResult<YoneticiSiteDetailVm>> GetYoneticiSiteByIdAsync(int id);
        Task<ServiceResult<object>> CreateYoneticiSiteAsync(YoneticiSiteVm dto);
        Task<ServiceResult<object>> UpdateYoneticiSiteAsync(YoneticiSiteDetailVm dto);
        Task<ServiceResult<object>> DeleteYoneticiSiteAsync(int id);
        Task<ServiceResult<List<YoneticiSiteDetailVm>>> GetYoneticiSitesByKeycloakUserIdAsync(string keycloakUserId);
    }
}