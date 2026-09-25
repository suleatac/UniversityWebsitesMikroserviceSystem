using Microservice.Web.ViewModels.Ogrenci;
using Refit;

namespace Microservice.Web.Clients.OgrenciClients
{
    public interface IOgrenciClientServices
    {
        // Ogrenci microservice'deki ogrenci listesi (sayaclar icin; alanlari eksiksiz doner,
        // web tarafi sadece ihtiyac duydugu alanlari esler).
        [Get("/api/v1/ogrencis")]
        Task<ApiResponse<List<OgrenciSayiVm>>> GetOgrencilerAsync();
    }
}
