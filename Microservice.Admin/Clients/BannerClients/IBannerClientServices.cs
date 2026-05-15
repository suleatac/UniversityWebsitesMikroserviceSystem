using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Banner;
using Refit;

namespace Microservice.Admin.Clients.BannerClients
{
    public interface IBannerClientServices
    {
        [Get("/api/v1/banners")]
        Task<ApiResponse<List<GetBannerVm>>> GetBannersAsync(int siteId, int dilId);

        [Get("/api/v1/banners/{id}")]
        Task<ApiResponse<BannerDetailVm>> GetBannerByIdAsync(int id);

        [Post("/api/v1/banners")]
        Task<ApiResponse<object>> CreateBannerAsync([Body] CreateBannerVm dto);

        [Put("/api/v1/banners/{id}")]
        Task<ApiResponse<object>> UpdateBannerAsync(int id, [Body] BannerDetailVm dto);

        [Delete("/api/v1/banners/{id}")]
        Task<ApiResponse<object>> DeleteBannerAsync(int id);

        [Get("/api/v1/banners/paginated")]
        Task<ApiResponse<PaginatedResult<GetBannerVm>>> GetBannersPaginatedAsync(
          int siteId, int dilId,
          [AliasAs("page")] int page,
          [AliasAs("pageSize")] int pageSize,
          [AliasAs("search")] string? search,
          [AliasAs("orderBy")] string? orderBy,
          [AliasAs("orderDir")] string? orderDir);

        [Put("/api/v1/banners/reorder")]
        Task<ApiResponse<object>> ReorderBannersAsync([Body] ReorderBannersCommandListVm items);
    }
}
