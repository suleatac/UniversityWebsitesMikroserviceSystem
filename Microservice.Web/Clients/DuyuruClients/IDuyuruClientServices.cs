using Microservice.Web.ViewModels.Duyuru;
using Microservice.Web.ViewModels.Paged;
using Refit;

namespace Microservice.Web.Clients.DuyuruClients
{
    public interface IDuyuruClientServices
    {
        [Get("/api/v1/duyurular")]
        Task<ApiResponse<List<GetDuyuruVm>>> GetDuyurularAsync(int siteId, int dilId);

        [Get("/api/v1/duyurular/{id}")]
        Task<ApiResponse<DuyuruDetailVm>> GetDuyuruByIdAsync(int id);

        [Get("/api/v1/duyurular/seo/{siteId}/{dilId}/{seoUrl}")]
        Task<ApiResponse<DuyuruDetailVm>> GetDuyuruBySeoUrlAsync(int siteId, int dilId, string seoUrl);

        // Sayfali + istege bagli arama destekli duyuru listesi
        [Get("/api/v1/duyurular/paginated")]
        Task<ApiResponse<PagedResultVm<GetDuyuruVm>>> GetPaginatedAsync(
            int siteId,
            int dilId,
            int page = 1,
            int pageSize = 10,
            string? search = null,
            string? orderBy = null,
            string? orderDir = null);
    }
}