using Microservice.Web.ViewModels.Icerik;
using Microservice.Web.ViewModels.Paged;
using Refit;

namespace Microservice.Web.Clients.IcerikClients
{
    public interface IIcerikClientServices
    {
        // Icerik tablosunda (Haber, Duyuru, Bilgi, Etkinlik, Video) sayfali arama.
        [Get("/api/v1/icerikler/search")]
        Task<ApiResponse<PagedResultVm<IcerikSearchVm>>> SearchAsync(
            int siteId,
            int dilId,
            string? search = null,
            int? tip = null,
            int page = 1,
            int pageSize = 5);
    }
}
