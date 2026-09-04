using Microservice.Web.ViewModels.Pages;
using Refit;

namespace Microservice.Web.Clients.PageTypeClients
{
    public interface IPageTypeClientServices
    {
  

        [Get("/api/v1/page-types/slug/{siteTemplateId}/{slug}")]
        Task<ApiResponse<PagesDetailVm>> GetPagesBySlugAsync(int siteTemplateId, int dilId, string slug);
        [Get("/api/v1/page-types/home/{siteTemplateId}/{dilId}")]
        Task<ApiResponse<PagesDetailVm>> HomePageControlAsync(int siteTemplateId, int dilId);
    }
}
