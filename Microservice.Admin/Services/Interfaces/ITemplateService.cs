using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Template;

namespace Microservice.Admin.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<ServiceResult<List<GetTemplateVm>>> GetTemplatesAsync();
        Task<ServiceResult<bool>> CreateTemplateAsync(CreateTemplateVm dto);
        Task<ServiceResult<bool>> DeleteTemplateAsync(int id);
        Task<ServiceResult<GetTemplateVm>> GetTemplateByIdAsync(int id);
    }
}
