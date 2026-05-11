using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SitePersonel;

namespace Microservice.Admin.Services.Interfaces
{
    public interface ISitePersonelService
    {
        Task<ServiceResult<List<GetSitePersonelVm>>> GetSitePersonellerAsync(int siteId);
        Task<ServiceResult<SitePersonelDetailVm>> GetSitePersonelByIdAsync(int id);
        Task<ServiceResult<object>> CreateSitePersonelAsync(CreateSitePersonelVm dto);
        Task<ServiceResult<object>> UpdateSitePersonelAsync(SitePersonelDetailVm dto);
        Task<ServiceResult<object>> DeleteSitePersonelAsync(int id);
    }
}