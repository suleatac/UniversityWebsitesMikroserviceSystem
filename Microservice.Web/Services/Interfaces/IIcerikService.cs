using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Icerik;
using Microservice.Web.ViewModels.Paged;

namespace Microservice.Web.Services.Interfaces
{
    public interface IIcerikService
    {
        Task<ServiceResult<PagedResultVm<IcerikSearchVm>>> SearchAsync(
            int siteId,
            int dilId,
            string? search,
            int? tip,
            int page,
            int pageSize);
    }
}
