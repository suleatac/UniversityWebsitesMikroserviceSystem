using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.Services.Interfaces
{
    public interface ISiteService
    {
        Task<ServiceResult<List<SiteGetVm>>> GetSitesAsync();
        Task<ServiceResult<SiteDetailGetVm>> GetSiteByIdAsync(int id);

    }
}
