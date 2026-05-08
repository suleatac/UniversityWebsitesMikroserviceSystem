using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.YonetimDuyuru;
using Refit;

namespace Microservice.Admin.Clients.YonetimDuyuruClients
{
    public interface IYonetimDuyuruClientServices
    {
        [Get("/api/v1/yonetimDuyuru")]
        Task<ApiResponse<List<YonetimDuyuruVm>>> GetYonetimDuyurusAsync();

        [Get("/api/v1/yonetimDuyuru/{id}")]
        Task<ApiResponse<YonetimDuyuruVm>> GetYonetimDuyuruByIdAsync(int id);

        [Post("/api/v1/yonetimDuyuru")]
        Task<ApiResponse<object>> CreateYonetimDuyuruAsync([Body] YonetimDuyuruVm dto);

        [Put("/api/v1/yonetimDuyuru/{id}")]
        Task<ApiResponse<object>> UpdateYonetimDuyuruAsync(int id, [Body] YonetimDuyuruVm dto);

        [Delete("/api/v1/yonetimDuyuru/{id}")]
        Task<ApiResponse<object>> DeleteYonetimDuyuruAsync(int id);

        [Get("/api/v1/yonetimDuyuru/paginated")]
        Task<ApiResponse<PaginatedResult<YonetimDuyuruVm>>> GetYonetimDuyuruPaginatedAsync(
          [AliasAs("page")] int page,
          [AliasAs("pageSize")] int pageSize,
          [AliasAs("search")] string? search,
          [AliasAs("orderBy")] string? orderBy,
          [AliasAs("orderDir")] string? orderDir);
    }
}
