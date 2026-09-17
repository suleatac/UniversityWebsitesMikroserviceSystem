using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.GaleriResim;
using Refit;

namespace Microservice.Admin.Clients.GaleriResimClients
{
    public interface IGaleriResimClientServices
    {
        [Get("/api/v1/galeri-resimler")]
        Task<ApiResponse<List<GetGaleriResimVm>>> GetGaleriResimlerAsync(int siteId, int dilId);

        [Get("/api/v1/galeri-resimler/{id}")]
        Task<ApiResponse<GaleriResimDetailVm>> GetGaleriResimByIdAsync(int id);

        [Post("/api/v1/galeri-resimler")]
        Task<ApiResponse<object>> CreateGaleriResimAsync([Body] CreateGaleriResimVm dto);

        [Put("/api/v1/galeri-resimler/{id}")]
        Task<ApiResponse<object>> UpdateGaleriResimAsync(int id, [Body] GaleriResimDetailVm dto);

        [Delete("/api/v1/galeri-resimler/{id}")]
        Task<ApiResponse<object>> DeleteGaleriResimAsync(int id);

        [Get("/api/v1/galeri-resimler/paginated")]
        Task<ApiResponse<PaginatedResult<GetGaleriResimVm>>> GetGaleriResimlerPaginatedAsync(
          int siteId, int dilId,
          [AliasAs("page")] int page,
          [AliasAs("pageSize")] int pageSize,
          [AliasAs("search")] string? search,
          [AliasAs("orderBy")] string? orderBy,
          [AliasAs("orderDir")] string? orderDir);
    }
}
