using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.SitePersonel;

namespace Microservice.Web.Services.Interfaces
{
    public interface ISitePersonelService
    {
        Task<ServiceResult<List<GetPersonelVm>>> GetPersonelListAsync(int siteId);
        Task<ServiceResult<PersonelDetailVm>> GetPersonelByIdAsync(int id);
    }
}
