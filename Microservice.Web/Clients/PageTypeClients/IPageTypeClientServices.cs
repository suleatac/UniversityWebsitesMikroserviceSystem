using Microservice.Web.ViewModels.Pages;
using Refit;

namespace Microservice.Web.Clients.PageTypeClients
{
    public interface IPageTypeClientServices
    {
  

        [Get("/api/v1/page-types/slug/{siteId}/{slug}")]
        Task<ApiResponse<PagesDetailVm>> GetPagesBySlugAsync(int siteId, int dilId, string slug);
        [Get("/api/v1/page-types/home/{siteId}/{dilId}")]
        Task<ApiResponse<PagesDetailVm>> HomePageControlAsync(int siteId, int dilId);
    }
}
