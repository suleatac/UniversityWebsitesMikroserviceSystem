using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Popup;

namespace Microservice.Web.Services.Interfaces
{
    public interface IPopupService
    {
        /// <summary>
        /// Site icin ana sayfa popup'i. Popup tanimli degilse Data null doner (hata sayilmaz).
        /// </summary>
        Task<ServiceResult<GetPopupVm?>> GetPopupAsync(int siteId);
    }
}
