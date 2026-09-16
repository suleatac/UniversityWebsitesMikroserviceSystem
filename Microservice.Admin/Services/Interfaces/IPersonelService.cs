using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Personel;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IPersonelService
    {
        Task<ServiceResult<List<GetPersonelVm>>> GetSitePersonellerAsync();
        Task<ServiceResult<PersonelDetailVm>> GetPersonelByIdAsync(int id);
    }
}
