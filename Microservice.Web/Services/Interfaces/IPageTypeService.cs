using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Pages;

namespace Microservice.Web.Services.Interfaces
{
    public interface IPageTypeService
    {
        // Sayfa turune (PageTypeKindEnum) gore PageType getirir. Orn: Search sayfasi slug'i.
        Task<ServiceResult<PagesDetailVm>> GetPageTypeByKindAsync(int templateId, int dilId, int pageTypeKind);
    }
}
