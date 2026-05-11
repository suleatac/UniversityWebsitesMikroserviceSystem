using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Duyuru;
using Refit;

namespace Microservice.Admin.Clients.DuyuruClients
{
    public interface IDuyuruClientServices
    {
        [Get("/api/v1/duyurular")]
        Task<ApiResponse<List<GetDuyuruVm>>> GetDuyurularAsync(int siteId, int dilId);

        [Get("/api/v1/duyurular/{id}")]
        Task<ApiResponse<DuyuruDetailVm>> GetDuyuruByIdAsync(int id);

        [Post("/api/v1/duyurular")]
        Task<ApiResponse<object>> CreateDuyuruAsync([Body] CreateDuyuruVm dto);

        [Put("/api/v1/duyurular/{id}")]
        Task<ApiResponse<object>> UpdateDuyuruAsync(int id, [Body] DuyuruDetailVm dto);

        [Delete("/api/v1/duyurular/{id}")]
        Task<ApiResponse<object>> DeleteDuyuruAsync(int id);

        [Get("/api/v1/duyurular/paginated")]
        Task<ApiResponse<PaginatedResult<GetDuyuruVm>>> GetDuyurularPaginatedAsync(
          int siteId, int dilId,
          [AliasAs("page")] int page,
          [AliasAs("pageSize")] int pageSize,
          [AliasAs("search")] string? search,
          [AliasAs("orderBy")] string? orderBy,
          [AliasAs("orderDir")] string? orderDir);
    }
}