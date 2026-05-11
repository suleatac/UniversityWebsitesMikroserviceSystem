using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Bilgi;
using Refit;

namespace Microservice.Admin.Clients.BilgiClients
{
    public interface IBilgiClientServices
    {
        [Get("/api/v1/bilgis")]
        Task<ApiResponse<List<GetBilgiVm>>> GetBilgilerAsync(int siteId, int dilId);

        [Get("/api/v1/bilgis/{id}")]
        Task<ApiResponse<BilgiDetailVm>> GetBilgiByIdAsync(int id);

        [Post("/api/v1/bilgis")]
        Task<ApiResponse<object>> CreateBilgiAsync([Body] CreateBilgiVm dto);

        [Put("/api/v1/bilgis/{id}")]
        Task<ApiResponse<object>> UpdateBilgiAsync(int id, [Body] BilgiDetailVm dto);

        [Delete("/api/v1/bilgis/{id}")]
        Task<ApiResponse<object>> DeleteBilgiAsync(int id);

        [Get("/api/v1/bilgis/paginated")]
        Task<ApiResponse<PaginatedResult<GetBilgiVm>>> GetBilgilerPaginatedAsync(
          int siteId, int dilId,
          [AliasAs("page")] int page,
          [AliasAs("pageSize")] int pageSize,
          [AliasAs("search")] string? search,
          [AliasAs("orderBy")] string? orderBy,
          [AliasAs("orderDir")] string? orderDir);
    }
}