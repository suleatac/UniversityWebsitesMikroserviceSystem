using Microservice.Web.ViewModels.PageRoute;
using Refit;

namespace Microservice.Web.Clients.PageRouteClients
{
    public interface IPageRouteClientServices
    {
  

        [Get("/api/v1/page-routes/slug/{siteId}/{slug}")]
        Task<ApiResponse<PageRouteSlugDetailVm>> GetPageRouteBySlugAsync(int siteId, string slug);
    }
}
