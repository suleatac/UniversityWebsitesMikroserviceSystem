using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Etkinlik;
using Refit;

namespace Microservice.Admin.Clients.EtkinlikClients
{
    public interface IEtkinlikClientServices
    {
        [Get("/api/v1/etkinlikler")]
        Task<ApiResponse<List<GetEtkinlikVm>>> GetEtkinliklerAsync(int siteId, int dilId);

        [Get("/api/v1/etkinlikler/{id}")]
        Task<ApiResponse<EtkinlikDetailVm>> GetEtkinlikByIdAsync(int id);

        [Post("/api/v1/etkinlikler")]
        Task<ApiResponse<object>> CreateEtkinlikAsync([Body] CreateEtkinlikVm dto);

        [Put("/api/v1/etkinlikler/{id}")]
        Task<ApiResponse<object>> UpdateEtkinlikAsync(int id, [Body] EtkinlikDetailVm dto);

        [Delete("/api/v1/etkinlikler/{id}")]
        Task<ApiResponse<object>> DeleteEtkinlikAsync(int id);

        [Get("/api/v1/etkinlikler/paginated")]
        Task<ApiResponse<PaginatedResult<GetEtkinlikVm>>> GetEtkinliklerPaginatedAsync(
          int siteId, int dilId,
          [AliasAs("page")] int page,
          [AliasAs("pageSize")] int pageSize,
          [AliasAs("search")] string? search,
          [AliasAs("orderBy")] string? orderBy,
          [AliasAs("orderDir")] string? orderDir);
    }
}