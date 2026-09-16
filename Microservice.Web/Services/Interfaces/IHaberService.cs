using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Paged;

namespace Microservice.Web.Services.Interfaces
{
    public interface IHaberService
    {
        Task<ServiceResult<List<GetHaberVm>>> GetHabersAsync(int siteId, int dilId);
        Task<ServiceResult<HaberDetailVm>> GetHaberByIdAsync(int id);

        // Sayfali + istege bagli arama destekli haber listesi
        Task<ServiceResult<PagedResultVm<GetHaberVm>>> GetPaginatedAsync(
            int siteId,
            int dilId,
            string? search,
            int page,
            int pageSize);
    }
}
