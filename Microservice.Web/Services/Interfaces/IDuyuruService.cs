using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Duyuru;

namespace Microservice.Web.Services.Interfaces
{
    public interface IDuyuruService
    {
        Task<ServiceResult<List<GetDuyuruVm>>> GetDuyurularAsync(int siteId, int dilId);
        Task<ServiceResult<DuyuruDetailVm>> GetDuyuruByIdAsync(int id);
       
    }
}