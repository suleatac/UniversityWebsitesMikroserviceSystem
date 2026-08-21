using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.Interfaces
{
    public interface IPageRouteService
    {
        Task<ServiceResult<PageRouteSlugDetailVm>> GetPageRouteBySlugAsync(int siteId, string slug);
    }
}
