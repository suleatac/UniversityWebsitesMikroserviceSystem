using Microservice.Web.ViewModels.GaleriResim;
using Microservice.Web.ViewModels.Paged;
using Refit;

namespace Microservice.Web.Clients.GaleriResimClients
{
    public interface IGaleriResimClientServices
    {
        [Get("/api/v1/galeri-resimler")]
        Task<ApiResponse<List<GetGaleriResimVm>>> GetGaleriResimlerAsync(int siteId, int dilId);

        [Get("/api/v1/galeri-resimler/{id}")]
        Task<ApiResponse<GaleriResimDetailVm>> GetGaleriResimByIdAsync(int id);

        [Get("/api/v1/galeri-resimler/seo/{siteId}/{dilId}/{seoUrl}")]
        Task<ApiResponse<GaleriResimDetailVm>> GetGaleriResimBySeoUrlAsync(int siteId, int dilId, string seoUrl);

        // Sayfali + istege bagli arama/kategori destekli galeri resmi listesi
        [Get("/api/v1/galeri-resimler/paginated")]
        Task<ApiResponse<PagedResultVm<GetGaleriResimVm>>> GetPaginatedAsync(
            int siteId,
            int dilId,
            int page = 1,
            int pageSize = 10,
            string? search = null,
            string? kategori = null,
            string? orderBy = null,
            string? orderDir = null);
    }
}
