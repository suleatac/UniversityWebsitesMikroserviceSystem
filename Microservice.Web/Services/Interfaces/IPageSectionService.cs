using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.PageSection;

namespace Microservice.Web.Services.Interfaces
{
    public interface IPageSectionService
    {
        /// <summary>
        /// Site+dil bazinda ana sayfa icin yayindaki dinamik bolumleri (block agaciyla) getirir.
        /// Bolum tanimli degilse bos liste doner (hata sayilmaz).
        /// </summary>
        Task<ServiceResult<List<GetPageSectionVm>>> GetPublishedSectionsAsync(int siteId, int dilId);
    }
}
