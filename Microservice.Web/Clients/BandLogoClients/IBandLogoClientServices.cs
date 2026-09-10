using Microservice.Web.ViewModels.BandLogo;
using Refit;

namespace Microservice.Web.Clients.BandLogoClients
{
    public interface IBandLogoClientServices
    {
        [Get("/api/v1/bandlogos")]
        Task<ApiResponse<List<GetBandLogoVm>>> GetBandLogosAsync(int siteId, int dilId);

        [Get("/api/v1/bandlogos/{id}")]
        Task<ApiResponse<BandLogoDetailVm>> GetBandLogoByIdAsync(int id);

      
    }
}
