using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.YonetimDuyuru;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IYonetimDuyuruService
    {
        Task<ServiceResult<List<YonetimDuyuruVm>>> GetYonetimDuyurusAsync();
        Task<ServiceResult<PaginatedResult<YonetimDuyuruVm>>> GetYonetimDuyuruPaginatedAsync(
     int page, int pageSize, string? search, string? orderBy, string? orderDir);
        Task<ServiceResult<YonetimDuyuruVm>> GetYonetimDuyuruByIdAsync(int id);
        Task<ServiceResult<object>> CreateYonetimDuyuruAsync(YonetimDuyuruVm dto);
        Task<ServiceResult<bool>> UpdateYonetimDuyuruAsync(YonetimDuyuruVm dto);
        Task<ServiceResult<bool>> DeleteYonetimDuyuruAsync(int id);
    }
}
