using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Site;

namespace Microservice.Admin.Services.Interfaces
{
    public interface ISiteService
    {
        Task<ServiceResult<List<SiteGetVm>>> GetSitesAsync();
    }
}
