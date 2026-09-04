using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.PageType;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IPageTypeService
    {
        Task<ServiceResult<List<GetPageTypeVm>>> GetPageTypesAsync();
        Task<ServiceResult<GetPageTypeVm>> GetPageTypeByIdAsync(int id);
        Task<ServiceResult<GetPageTypeVm>> GetPageTypeByTemplateIdAndPageTypeKindAsync(
            int templateId, int dilId, PageTypeKind pageTypeKind);
        Task<ServiceResult<bool>> CreatePageTypeAsync(CreatePageTypeVm model);
        Task<ServiceResult<bool>> UpdatePageTypeAsync(UpdatePageTypeVm model);
        Task<ServiceResult<bool>> DeletePageTypeAsync(int id);
    }
}