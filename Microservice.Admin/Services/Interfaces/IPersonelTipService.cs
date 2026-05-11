using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.PersonelTip;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IPersonelTipService
    {
        Task<ServiceResult<List<GetPersonelTipVm>>> GetPersonelTiplerAsync();
        Task<ServiceResult<PersonelTipVm>> GetPersonelTipByIdAsync(int id);
        Task<ServiceResult<bool>> CreatePersonelTipAsync(PersonelTipVm dto);
        Task<ServiceResult<bool>> UpdatePersonelTipAsync(PersonelTipVm dto);
        Task<ServiceResult<bool>> DeletePersonelTipAsync(int id);
    }
}