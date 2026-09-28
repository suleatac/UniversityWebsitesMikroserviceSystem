using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.PageBuilder;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IPageSectionService
    {
        Task<ServiceResult<List<GetPageSectionVm>>> GetPageSectionsAsync(int siteId, int dilId);
        Task<ServiceResult<GetPageSectionVm>> GetPageSectionByIdAsync(int id);
        Task<ServiceResult<object>> CreatePageSectionAsync(PageSectionDetailVm dto);
        Task<ServiceResult<object>> UpdatePageSectionAsync(PageSectionDetailVm dto);
        Task<ServiceResult<object>> DeletePageSectionAsync(int id);
        Task<ServiceResult<object>> ReorderPageSectionsAsync(List<ReorderPageSectionItemVm> items);
        Task<ServiceResult<object>> CreatePageBlockAsync(int sectionId, PageBlockFormVm dto);
        Task<ServiceResult<object>> UpdatePageBlockAsync(PageBlockFormVm dto);
        Task<ServiceResult<object>> DeletePageBlockAsync(int blockId);
    }
}
