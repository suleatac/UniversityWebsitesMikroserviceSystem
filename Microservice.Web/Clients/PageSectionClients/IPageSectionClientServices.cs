using Microservice.Web.ViewModels.PageSection;
using Refit;

namespace Microservice.Web.Clients.PageSectionClients
{
    public interface IPageSectionClientServices
    {
        // Ana sayfa icin site+dil bazinda YAYINDAKI bolumleri (block agaciyla) dondurur.
        [Get("/api/v1/page-sections?siteId={siteId}&dilId={dilId}&publishedOnly=true")]
        Task<ApiResponse<List<GetPageSectionVm>>> GetPublishedPageSectionsAsync(int siteId, int dilId);
    }
}
