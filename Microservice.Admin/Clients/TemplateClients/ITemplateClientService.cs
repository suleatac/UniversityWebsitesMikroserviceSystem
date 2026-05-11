using Microservice.Admin.ViewModels.Template;
using Refit;

namespace Microservice.Admin.Clients.TemplateClients
{
    public interface ITemplateClientService
    {
        [Get("/api/v1/templates")]
        Task<ApiResponse<List<GetTemplateVm>>> GetTemplatesAsync();

        [Get("/api/v1/templates/{id}")]
        Task<ApiResponse<GetTemplateVm>> GetTemplateByIdAsync(int id);

        [Post("/api/v1/templates")]
        Task<ApiResponse<object>> CreateTemplateAsync(CreateTemplateVm dto);

        [Put("/api/v1/templates")]
        Task<ApiResponse<object>> UpdateTemplateAsync(UpdateTemplateVm dto);

        [Delete("/api/v1/templates/{id}")]
        Task<ApiResponse<object>> DeleteTemplateAsync(int id);
    }
}
