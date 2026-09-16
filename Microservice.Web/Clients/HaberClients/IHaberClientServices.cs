using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Paged;
using Refit;

namespace Microservice.Web.Clients.HaberClients
{
    public interface IHaberClientServices
    {
        [Get("/api/v1/habers")]
        Task<ApiResponse<List<GetHaberVm>>> GetHabersAsync(int siteId, int dilId);

        [Get("/api/v1/habers/{id}")]
        Task<ApiResponse<HaberDetailVm>> GetHaberByIdAsync(int id);

        [Get("/api/v1/habers/seo/{siteId}/{dilId}/{seoUrl}")]
        Task<ApiResponse<HaberDetailVm>> GetHaberBySeoUrlAsync(int siteId, int dilId, string seoUrl);

        // Sayfali + istege bagli arama destekli haber listesi
        [Get("/api/v1/habers/paginated")]
        Task<ApiResponse<PagedResultVm<GetHaberVm>>> GetPaginatedAsync(
            int siteId,
            int dilId,
            int page = 1,
            int pageSize = 10,
            string? search = null,
            string? orderBy = null,
            string? orderDir = null);
    }
}
