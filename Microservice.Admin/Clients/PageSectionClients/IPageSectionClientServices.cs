using Microservice.Admin.ViewModels.PageBuilder;
using Refit;

namespace Microservice.Admin.Clients.PageSectionClients
{
    public interface IPageSectionClientServices
    {
        // Admin: site+dil bazinda TUM bolumler (yayinda + yayinda olmayan) block agaciyla.
        [Get("/api/v1/page-sections?siteId={siteId}&dilId={dilId}&publishedOnly=false")]
        Task<ApiResponse<List<GetPageSectionVm>>> GetPageSectionsAsync(int siteId, int dilId);

        [Get("/api/v1/page-sections/{id}")]
        Task<ApiResponse<GetPageSectionVm>> GetPageSectionByIdAsync(int id);

        [Post("/api/v1/page-sections")]
        Task<ApiResponse<object>> CreatePageSectionAsync([Body] PageSectionDetailVm dto);

        [Put("/api/v1/page-sections/{id}")]
        Task<ApiResponse<object>> UpdatePageSectionAsync(int id, [Body] PageSectionDetailVm dto);

        [Delete("/api/v1/page-sections/{id}")]
        Task<ApiResponse<object>> DeletePageSectionAsync(int id);

        [Put("/api/v1/page-sections/reorder")]
        Task<ApiResponse<object>> ReorderPageSectionsAsync([Body] ReorderPageSectionsCommandListVm items);

        // Container (block) islemleri: section id altinda POST; block id ile PUT/DELETE.
        [Post("/api/v1/page-sections/{sectionId}/blocks")]
        Task<ApiResponse<object>> CreatePageBlockAsync(int sectionId, [Body] PageBlockFormVm dto);

        [Put("/api/v1/page-sections/blocks/{blockId}")]
        Task<ApiResponse<object>> UpdatePageBlockAsync(int blockId, [Body] PageBlockFormVm dto);

        [Delete("/api/v1/page-sections/blocks/{blockId}")]
        Task<ApiResponse<object>> DeletePageBlockAsync(int blockId);
    }
}
