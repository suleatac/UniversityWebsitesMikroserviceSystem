using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Etkinlik;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IEtkinlikService
    {
        Task<ServiceResult<List<GetEtkinlikVm>>> GetEtkinliklerAsync(int siteId, int dilId);
        Task<ServiceResult<EtkinlikDetailVm>> GetEtkinlikByIdAsync(int id);
        Task<ServiceResult<object>> CreateEtkinlikAsync(CreateEtkinlikVm dto);
        Task<ServiceResult<object>> UpdateEtkinlikAsync(EtkinlikDetailVm dto);
        Task<ServiceResult<object>> DeleteEtkinlikAsync(int id);
        Task<ServiceResult<PaginatedResult<GetEtkinlikVm>>> GetEtkinliklerPaginatedAsync(
            int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir);
    }
}