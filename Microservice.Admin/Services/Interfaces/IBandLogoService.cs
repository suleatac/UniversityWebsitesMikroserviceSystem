using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.BandLogo;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IBandLogoService
    {
        Task<ServiceResult<List<GetBandLogoVm>>> GetBandLogosAsync(int siteId, int dilId);
        Task<ServiceResult<BandLogoDetailVm>> GetBandLogoByIdAsync(int id);
        Task<ServiceResult<object>> CreateBandLogoAsync(CreateBandLogoVm dto);
        Task<ServiceResult<object>> UpdateBandLogoAsync(BandLogoDetailVm dto);
        Task<ServiceResult<object>> DeleteBandLogoAsync(int id);
        Task<ServiceResult<PaginatedResult<GetBandLogoVm>>> GetBandLogosPaginatedAsync(
            int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir);
    }
}
