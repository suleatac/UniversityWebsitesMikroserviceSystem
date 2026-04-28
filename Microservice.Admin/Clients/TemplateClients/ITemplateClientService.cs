using Microservice.Admin.ViewModels.Template;
using Refit;

namespace Microservice.Admin.Clients.TemplateClients
{
    public interface ITemplateClientService
    {
        [Get("/api/v1/templates")]
        Task<ApiResponse<List<GetTemplateVm>>> GetTemplatesAsync();


        [Post("/api/v1/templates")]
        Task<ApiResponse<object>> CreateTemplateAsync(CreateTemplateVm dto);

        
    }
}
