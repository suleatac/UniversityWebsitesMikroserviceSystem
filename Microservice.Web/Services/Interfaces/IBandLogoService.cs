using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.BandLogo;

namespace Microservice.Web.Services.Interfaces
{
    public interface IBandLogoService
    {
        Task<ServiceResult<List<GetBandLogoVm>>> GetBandLogosAsync(int siteId, int dilId);
        Task<ServiceResult<BandLogoDetailVm>> GetBandLogoByIdAsync(int id);
       
    }
}
