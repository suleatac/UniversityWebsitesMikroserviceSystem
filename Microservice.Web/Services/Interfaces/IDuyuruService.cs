using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Duyuru;
using Microservice.Web.ViewModels.Paged;

namespace Microservice.Web.Services.Interfaces
{
    public interface IDuyuruService
    {
        Task<ServiceResult<List<GetDuyuruVm>>> GetDuyurularAsync(int siteId, int dilId);
        Task<ServiceResult<DuyuruDetailVm>> GetDuyuruByIdAsync(int id);

        // Sayfali + istege bagli arama destekli duyuru listesi
        Task<ServiceResult<PagedResultVm<GetDuyuruVm>>> GetPaginatedAsync(
            int siteId,
            int dilId,
            string? search,
            int page,
            int pageSize);
    }
}