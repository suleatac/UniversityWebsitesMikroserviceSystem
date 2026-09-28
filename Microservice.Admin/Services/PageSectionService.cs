using Microservice.Admin.Clients.PageSectionClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.PageBuilder;

namespace Microservice.Admin.Services
{
    public class PageSectionService : IPageSectionService
    {
        private readonly IPageSectionClientServices _pageSectionClient;
        private readonly ILogger<PageSectionService> _logger;

        public PageSectionService(
            IPageSectionClientServices pageSectionClient,
            ILogger<PageSectionService> logger)
        {
            _pageSectionClient = pageSectionClient;
            _logger = logger;
        }

        public async Task<ServiceResult<List<GetPageSectionVm>>> GetPageSectionsAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Sayfa bölümleri getiriliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _pageSectionClient.GetPageSectionsAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Detail: {Detail}", response.StatusCode, problemDetails);
                return ServiceResult<List<GetPageSectionVm>>.Error(problemDetails ?? "Bölüm listesi alınamadı");
            }

            return ServiceResult<List<GetPageSectionVm>>.Success(response.Content ?? []);
        }

        public async Task<ServiceResult<GetPageSectionVm>> GetPageSectionByIdAsync(int id)
        {
            var response = await _pageSectionClient.GetPageSectionByIdAsync(id);

            if (!response.IsSuccessStatusCode || response.Content is null)
            {
                var problemDetails = response.Error?.Content;
                return ServiceResult<GetPageSectionVm>.Error(problemDetails ?? "Bölüm bulunamadı");
            }

            return ServiceResult<GetPageSectionVm>.Success(response.Content);
        }

        public async Task<ServiceResult<object>> CreatePageSectionAsync(PageSectionDetailVm dto)
        {
            _logger.LogInformation("Yeni bölüm oluşturuluyor. Başlık: {Title}", dto.Baslik);
            var response = await _pageSectionClient.CreatePageSectionAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Detail: {Detail}", response.StatusCode, problemDetails);
                return ServiceResult<object>.Error(problemDetails ?? "Bölüm oluşturulamadı");
            }

            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdatePageSectionAsync(PageSectionDetailVm dto)
        {
            _logger.LogInformation("Bölüm güncelleniyor. Id: {Id}", dto.Id);
            var response = await _pageSectionClient.UpdatePageSectionAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Detail: {Detail}", response.StatusCode, problemDetails);
                return ServiceResult<object>.Error(problemDetails ?? $"Bölüm güncellenemedi. Id: {dto.Id}");
            }

            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeletePageSectionAsync(int id)
        {
            _logger.LogWarning("Bölüm silme isteği alındı. Id: {Id}", id);
            var response = await _pageSectionClient.DeletePageSectionAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content;
                return ServiceResult<object>.Error(problemDetails ?? "Bölüm silinemedi");
            }

            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> ReorderPageSectionsAsync(List<ReorderPageSectionItemVm> items)
        {
            _logger.LogInformation("Bölüm sıralaması güncelleniyor. Öğe sayısı: {Count}", items?.Count ?? 0);
            var newReorderList = new ReorderPageSectionsCommandListVm {
                Items = items ?? []
            };
            var response = await _pageSectionClient.ReorderPageSectionsAsync(newReorderList);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content;
                return ServiceResult<object>.Error(problemDetails ?? "Sıralama güncellenemedi");
            }

            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> CreatePageBlockAsync(int sectionId, PageBlockFormVm dto)
        {
            _logger.LogInformation("Yeni container oluşturuluyor. SectionId: {SectionId}, Tip: {Type}", sectionId, dto.ContentType);
            var response = await _pageSectionClient.CreatePageBlockAsync(sectionId, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Detail: {Detail}", response.StatusCode, problemDetails);
                return ServiceResult<object>.Error(problemDetails ?? "Container oluşturulamadı");
            }

            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdatePageBlockAsync(PageBlockFormVm dto)
        {
            _logger.LogInformation("Container güncelleniyor. Id: {Id}", dto.Id);
            var response = await _pageSectionClient.UpdatePageBlockAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content;
                return ServiceResult<object>.Error(problemDetails ?? $"Container güncellenemedi. Id: {dto.Id}");
            }

            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeletePageBlockAsync(int blockId)
        {
            _logger.LogWarning("Container silme isteği alındı. Id: {Id}", blockId);
            var response = await _pageSectionClient.DeletePageBlockAsync(blockId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content;
                return ServiceResult<object>.Error(problemDetails ?? "Container silinemedi");
            }

            return ServiceResult<object>.Success(true);
        }
    }
}
