using Microservice.Web.ViewModels.Duyuru;
using Microservice.Web.ViewModels.Etkinlik;
using Refit;

namespace Microservice.Web.Clients.EtkinlikClients
{
    public interface IEtkinlikClientServices
    {

        [Get("/api/v1/etkinlikler/{id}")]
        Task<ApiResponse<EtkinlikDetailVm>> GetEtkinlikByIdAsync(int id);

        [Get("/api/v1/etkinlikler")]
        Task<ApiResponse<List<GetEtkinlikVm>>> GetEtkinliklerAsync(int siteId, int dilId);

        [Get("/api/v1/etkinlikler/seo/{siteId}/{dilId}/{seoUrl}")]
        Task<ApiResponse<EtkinlikDetailVm>> GetEtkinlikBySeoUrlAsync(int siteId, int dilId, string seoUrl);
    }
}
