using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SiteOzellikleri;

namespace Microservice.Admin.Services.Interfaces
{
    public interface ISiteOzellikleriService
    {
        Task<ServiceResult<SiteOzellikleriVm>> GetSiteOzellikleriAsync(int siteId);
        Task<ServiceResult<object>> CreateSiteOzellikleriAsync(SiteOzellikleriVm dto);
        Task<ServiceResult<object>> UpdateSiteOzellikleriAsync(int id, SiteOzellikleriVm dto);
        Task<ServiceResult<object>> DeleteSiteOzellikleriAsync(int id);
    }
}