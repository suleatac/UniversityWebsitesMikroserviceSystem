using Microservice.Admin.Clients.HaberClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Haber;
using Microservice.Admin.ViewModels.PageType;
using Microservice.Admin.ViewModels.Site;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class HaberService : IHaberService
    {
        private readonly IHaberClientService _haberClient;
        private readonly ILogger<HaberService> _logger;
        private readonly IPageTypeService _pageTypeService;
        private readonly ISiteService _siteService;
        private readonly ISeoService _seoService;

        public HaberService(IHaberClientService haberClient, ILogger<HaberService> logger, IPageTypeService pageTypeService, ISiteService siteService, ISeoService seoService)
        {
            _haberClient = haberClient ?? throw new ArgumentNullException(nameof(haberClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pageTypeService = pageTypeService ?? throw new ArgumentNullException(nameof(pageTypeService));
            _siteService = siteService ?? throw new ArgumentNullException(nameof(siteService));
            _seoService = seoService ?? throw new ArgumentNullException(nameof(seoService));
        }

        // LIST
        public async Task<ServiceResult<List<GetHaberVm>>> GetHabersAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Haber listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);

            var response = await _haberClient.GetHabersAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<List<GetHaberVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Haberler alınamadı"
                );
            }

            _logger.LogInformation("Haber listesi alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<GetHaberVm>>.Success(response.Content!);
        }



        //Haber paginated list
        public async Task<ServiceResult<PaginatedResult<GetHaberVm>>> GetHabersPaginatedAsync
            (
               int siteId, 
               int dilId,
               int page, 
               int pageSize, 
               string? search, 
               string? orderBy, 
               string? orderDir
            )
        {
            _logger.LogInformation("API'den paginated haber listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);

            var response = await _haberClient.GetHabersPaginatedAsync(siteId, dilId, page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}",
                    response.StatusCode, problemDetails?.Title);

                return ServiceResult<PaginatedResult<GetHaberVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated haber listesi alınamadı");
            }

            _logger.LogInformation("Paginated haber listesi başarıyla alındı. TotalCount: {TotalCount}",
                response.Content?.TotalCount);

            return ServiceResult<PaginatedResult<GetHaberVm>>.Success(response.Content!);
        }


        // GET BY ID
        public async Task<ServiceResult<HaberDetailVm>> GetHaberByIdAsync(int id)
        {
            _logger.LogInformation("Haber getiriliyor. Id: {Id}", id);

            var response = await _haberClient.GetHaberByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<HaberDetailVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Haber bulunamadı"
                );
            }

            return ServiceResult<HaberDetailVm>.Success(response.Content!);
        }

        // CREATE
        public async Task<ServiceResult<object>> CreateHaberAsync(CreateHaberVm dto)
        {
  

            var siteResult = await _siteService.GetSiteByIdAsync(dto.SiteId);

            if (!siteResult.IsSuccess || siteResult.Data == null)
                return ServiceResult<object>.Error(
                    siteResult.Fail?.Detail ??
                    siteResult.Fail?.Title ??
                    "Site bilgisi alınamadı");

            var haberPageTypeResult =
                await _pageTypeService.GetPageTypeByTemplateIdAndPageTypeKindAsync(
                    siteResult.Data.TemplateId,
                    dto.DilId,
                    PageTypeKind.Haber);

            if (!haberPageTypeResult.IsSuccess || haberPageTypeResult.Data == null)
                return ServiceResult<object>.Error(
                    haberPageTypeResult.Fail?.Detail ??
                    haberPageTypeResult.Fail?.Title ??
                    "Haber sayfa türü bulunamadı");

            // PageTypeId'yi client'tan değil server'dan belirle
            dto.PageTypeId = haberPageTypeResult.Data.Id;

            // SEO bilgileri kullanıcıdan alınmaz; başlıktan otomatik üretilir
            await _seoService.ApplyAutoSeoAsync(dto.SiteId, dto.PageTypeId, dto.Baslik, dto.KisaAciklama,
                seoUrl => dto.SeoUrl = seoUrl,
                seoTitle => dto.SeoTitle = seoTitle,
                seoDescription => dto.SeoDescription = seoDescription,
                fallbackSlug: "haber");

            _logger.LogInformation("Yeni haber oluşturuluyor. Başlık: {Title}", dto.Baslik);
            var response = await _haberClient.CreateHaberAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<object>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Haber oluşturulamadı"
                );
            }

            _logger.LogInformation("Haber oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        // UPDATE
        public async Task<ServiceResult<object>> UpdateHaberAsync(HaberDetailVm dto)
        {


            var siteResult = await _siteService.GetSiteByIdAsync(dto.SiteId);

            if (!siteResult.IsSuccess || siteResult.Data == null)
                return ServiceResult<object>.Error(
                    siteResult.Fail?.Detail ??
                    siteResult.Fail?.Title ??
                    "Site bilgisi alınamadı");

            var haberTypeResult =
                await _pageTypeService.GetPageTypeByTemplateIdAndPageTypeKindAsync(
                    siteResult.Data.TemplateId,
                    dto.DilId,
                    PageTypeKind.Haber);

            if (!haberTypeResult.IsSuccess || haberTypeResult.Data == null)
                return ServiceResult<object>.Error(
                    haberTypeResult.Fail?.Detail ??
                    haberTypeResult.Fail?.Title ??
                    "Haber sayfa türü bulunamadı");

            // PageTypeId'yi client'tan değil server'dan belirle
            dto.PageTypeId = haberTypeResult.Data.Id;

            // SEO bilgileri kullanıcıdan alınmaz; başlıktan otomatik üretilir
            await _seoService.ApplyAutoSeoAsync(dto.SiteId, dto.PageTypeId, dto.Baslik, dto.KisaAciklama,
                seoUrl => dto.SeoUrl = seoUrl,
                seoTitle => dto.SeoTitle = seoTitle,
                seoDescription => dto.SeoDescription = seoDescription,
                fallbackSlug: "haber",
                excludeIcerikId: dto.Id);

            _logger.LogInformation("Haber güncelleniyor. Id: {Id}", dto.Id);

            var response = await _haberClient.UpdateHaberAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<object>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Haber güncellenemedi. Id: {dto.Id}"
                );
            }

            _logger.LogInformation("Haber güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        // DELETE
        public async Task<ServiceResult<object>> DeleteHaberAsync(int id)
        {
            _logger.LogWarning("Haber siliniyor. Id: {Id}", id);

            var response = await _haberClient.DeleteHaberAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<object>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Haber silinemedi. Id: {id}"
                );
            }

            _logger.LogInformation("Haber silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }
    }
}
