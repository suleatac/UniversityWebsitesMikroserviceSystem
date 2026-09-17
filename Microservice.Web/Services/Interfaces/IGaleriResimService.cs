using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.GaleriResim;
using Microservice.Web.ViewModels.Paged;

namespace Microservice.Web.Services.Interfaces
{
    public interface IGaleriResimService
    {
        Task<ServiceResult<List<GetGaleriResimVm>>> GetGaleriResimlerAsync(int siteId, int dilId);
        Task<ServiceResult<GaleriResimDetailVm>> GetGaleriResimByIdAsync(int id);

        // Sayfali + istege bagli arama/kategori destekli galeri resmi listesi
        Task<ServiceResult<PagedResultVm<GetGaleriResimVm>>> GetPaginatedAsync(
            int siteId,
            int dilId,
            string? search,
            string? kategori,
            int page,
            int pageSize);
    }
}
