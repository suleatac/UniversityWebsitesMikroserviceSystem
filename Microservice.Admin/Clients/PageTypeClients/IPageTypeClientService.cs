using Microservice.Admin.ViewModels.PageType;
using Refit;

namespace Microservice.Admin.Clients.PageTypeClients
{
    public interface IPageTypeClientService
    {
        [Get("/api/v1/page-types")]
        Task<ApiResponse<List<GetPageTypeVm>>> GetPageTypesAsync();

        [Get("/api/v1/page-types/{id}")]
        Task<ApiResponse<GetPageTypeVm>> GetPageTypeByIdAsync(int id);

        [Get("/api/v1/page-types/by-kind/{templateId}/{dilId}/{pageTypeKind}")]
        Task<ApiResponse<GetPageTypeVm>> GetPageTypeByTemplateIdAndPageTypeKindAsync(
            int templateId, int dilId, int pageTypeKind);

        [Post("/api/v1/page-types")]
        Task<ApiResponse<object>> CreatePageTypeAsync(CreatePageTypeVm model);

        [Put("/api/v1/page-types/{id}")]
        Task<ApiResponse<object>> UpdatePageTypeAsync(int id, UpdatePageTypeVm model);

        [Delete("/api/v1/page-types/{id}")]
        Task<ApiResponse<object>> DeletePageTypeAsync(int id);
    }
}