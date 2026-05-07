using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Haber;
using Refit;

namespace Microservice.Admin.Clients.HaberClients
{
    public interface IHaberClientService
    {
        [Get("/api/v1/habers")]
        Task<ApiResponse<List<GetHaberVm>>> GetHabersAsync(int siteId, int dilId);

        [Get("/api/v1/habers/{id}")]
        Task<ApiResponse<HaberDetailVm>> GetHaberByIdAsync(int id);

        [Post("/api/v1/habers")]
        Task<ApiResponse<object>> CreateHaberAsync([Body] CreateHaberVm dto);

        [Put("/api/v1/habers/{id}")]
        Task<ApiResponse<object>> UpdateHaberAsync(int id, [Body] HaberDetailVm dto);

        [Delete("/api/v1/habers/{id}")]
        Task<ApiResponse<object>> DeleteHaberAsync(int id);
        [Get("/api/v1/habers/paginated")]
        Task<ApiResponse<PaginatedResult<GetHaberVm>>> GetHabersPaginatedAsync(
          int siteId, int dilId,
          [AliasAs("page")] int page,
          [AliasAs("pageSize")] int pageSize,
          [AliasAs("search")] string? search,
          [AliasAs("orderBy")] string? orderBy,
          [AliasAs("orderDir")] string? orderDir);
    }
}
