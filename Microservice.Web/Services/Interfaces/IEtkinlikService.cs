using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Etkinlik;
using Microservice.Web.ViewModels.Paged;

namespace Microservice.Web.Services.Interfaces
{
    public interface IEtkinlikService
    {
        Task<ServiceResult<List<GetEtkinlikVm>>> GetEtkinliklerAsync(int siteId, int dilId);
        Task<ServiceResult<EtkinlikDetailVm>> GetEtkinlikByIdAsync(int id);

        // Sayfali + sayfa ici arama destekli etkinlik listesi (EtkinlikListesi sayfasi).
        Task<ServiceResult<PagedResultVm<GetEtkinlikVm>>> GetPaginatedAsync(
            int siteId,
            int dilId,
            string? search,
            int page,
            int pageSize);
    }
}
