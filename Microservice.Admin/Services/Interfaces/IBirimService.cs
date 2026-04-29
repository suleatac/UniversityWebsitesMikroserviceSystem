using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Birim;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IBirimService
    {
        Task<ServiceResult<List<GetBirimVm>>> GetBirimsAsync();
        Task<ServiceResult<GetBirimDetailVm>> GetBirimByIdAsync(int id);
        Task<ServiceResult<bool>> CreateBirimAsync(CreateBirimVm dto);
        Task<ServiceResult<bool>> UpdateBirimAsync(UpdateBirimVm dto);
        Task<ServiceResult<bool>> DeleteBirimAsync(int id);
    }
}
