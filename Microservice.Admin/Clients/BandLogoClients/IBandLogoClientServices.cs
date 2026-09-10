using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.BandLogo;
using Refit;

namespace Microservice.Admin.Clients.BandLogoClients
{
    public interface IBandLogoClientServices
    {
        [Get("/api/v1/bandlogos")]
        Task<ApiResponse<List<GetBandLogoVm>>> GetBandLogosAsync(int siteId, int dilId);

        [Get("/api/v1/bandlogos/{id}")]
        Task<ApiResponse<BandLogoDetailVm>> GetBandLogoByIdAsync(int id);

        [Post("/api/v1/bandlogos")]
        Task<ApiResponse<object>> CreateBandLogoAsync([Body] CreateBandLogoVm dto);

        [Put("/api/v1/bandlogos/{id}")]
        Task<ApiResponse<object>> UpdateBandLogoAsync(int id, [Body] BandLogoDetailVm dto);

        [Delete("/api/v1/bandlogos/{id}")]
        Task<ApiResponse<object>> DeleteBandLogoAsync(int id);

        [Get("/api/v1/bandlogos/paginated")]
        Task<ApiResponse<PaginatedResult<GetBandLogoVm>>> GetBandLogosPaginatedAsync(
          int siteId, int dilId,
          [AliasAs("page")] int page,
          [AliasAs("pageSize")] int pageSize,
          [AliasAs("search")] string? search,
          [AliasAs("orderBy")] string? orderBy,
          [AliasAs("orderDir")] string? orderDir);
    }
}
