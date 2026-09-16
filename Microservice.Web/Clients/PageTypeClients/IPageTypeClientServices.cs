using Microservice.Web.ViewModels.Pages;
using Refit;

namespace Microservice.Web.Clients.PageTypeClients
{
    public interface IPageTypeClientServices
    {
  

        [Get("/api/v1/page-types/slug/{siteTemplateId}/{dilId}/{slug}")]
        Task<ApiResponse<PagesDetailVm>> GetPagesBySlugAsync(int siteTemplateId, int dilId, string slug);
        [Get("/api/v1/page-types/home/{siteTemplateId}/{dilId}")]
        Task<ApiResponse<PagesDetailVm>> HomePageControlAsync(int siteTemplateId, int dilId);

        // pageTypeKind: Microservice.Web.Settings.PageTypeKindEnum karsiligi (orn: 15 = Search)
        [Get("/api/v1/page-types/by-kind/{siteTemplateId}/{dilId}/{pageTypeKind}")]
        Task<ApiResponse<PagesDetailVm>> GetPageTypeByKindAsync(int siteTemplateId, int dilId, int pageTypeKind);
    }
}
