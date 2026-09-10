using Microservice.Admin.Clients.EtkinlikClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Etkinlik;
using Microservice.Admin.ViewModels.PageType;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class EtkinlikService : IEtkinlikService
    {
        private readonly IEtkinlikClientServices _etkinlikClient;
        private readonly ILogger<EtkinlikService> _logger;
        private readonly IPageTypeService _pageTypeService;
        private readonly ISiteService _siteService;
        private readonly ISeoService _seoService;
        public EtkinlikService(IEtkinlikClientServices etkinlikClient, ILogger<EtkinlikService> logger, IPageTypeService pageTypeService, ISiteService siteService, ISeoService seoService)
        {
            _etkinlikClient = etkinlikClient ?? throw new ArgumentNullException(nameof(etkinlikClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pageTypeService = pageTypeService ?? throw new ArgumentNullException(nameof(pageTypeService));
            _siteService = siteService ?? throw new ArgumentNullException(nameof(siteService));
            _seoService = seoService ?? throw new ArgumentNullException(nameof(seoService));
        }

        public async Task<ServiceResult<List<GetEtkinlikVm>>> GetEtkinliklerAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Etkinlik listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _etkinlikClient.GetEtkinliklerAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetEtkinlikVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Etkinlikler alınamadı");
            }

            return ServiceResult<List<GetEtkinlikVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<EtkinlikDetailVm>> GetEtkinlikByIdAsync(int id)
        {
            _logger.LogInformation("Etkinlik getiriliyor. Id: {Id}", id);
            var response = await _etkinlikClient.GetEtkinlikByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<EtkinlikDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Etkinlik bulunamadı");
            }

            return ServiceResult<EtkinlikDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreateEtkinlikAsync(CreateEtkinlikVm dto)
        {


            var siteResult = await _siteService.GetSiteByIdAsync(dto.SiteId);

            if (!siteResult.IsSuccess || siteResult.Data == null)
                return ServiceResult<object>.Error(
                    siteResult.Fail?.Detail ??
                    siteResult.Fail?.Title ??
                    "Site bilgisi alınamadı");

            var etkinlikPageTypeResult =
                await _pageTypeService.GetPageTypeByTemplateIdAndPageTypeKindAsync(
                    siteResult.Data.TemplateId,
                    dto.DilId,
                    PageTypeKind.Etkinlik);

            if (!etkinlikPageTypeResult.IsSuccess || etkinlikPageTypeResult.Data == null)
                return ServiceResult<object>.Error(
                    etkinlikPageTypeResult.Fail?.Detail ??
                    etkinlikPageTypeResult.Fail?.Title ??
                    "Etkinlik sayfa türü bulunamadı");

            // PageTypeId'yi client'tan değil server'dan belirle
            dto.PageTypeId = etkinlikPageTypeResult.Data.Id;

            // SEO bilgileri kullanıcıdan alınmaz; başlıktan otomatik üretilir
            await _seoService.ApplyAutoSeoAsync(dto.SiteId, dto.PageTypeId, dto.Baslik, dto.KisaAciklama,
                seoUrl => dto.SeoUrl = seoUrl,
                seoTitle => dto.SeoTitle = seoTitle,
                seoDescription => dto.SeoDescription = seoDescription,
                fallbackSlug: "etkinlik");

            _logger.LogInformation("Yeni etkinlik oluşturuluyor. Başlık: {Title}", dto.Baslik);
            var response = await _etkinlikClient.CreateEtkinlikAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Etkinlik oluşturulamadı");
            }

            _logger.LogInformation("Etkinlik oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdateEtkinlikAsync(EtkinlikDetailVm dto)
        {





            var siteResult = await _siteService.GetSiteByIdAsync(dto.SiteId);

            if (!siteResult.IsSuccess || siteResult.Data == null)
                return ServiceResult<object>.Error(
                    siteResult.Fail?.Detail ??
                    siteResult.Fail?.Title ??
                    "Site bilgisi alınamadı");

            var etkinlikPageTypeResult =
                await _pageTypeService.GetPageTypeByTemplateIdAndPageTypeKindAsync(
                    siteResult.Data.TemplateId,
                    dto.DilId,
                    PageTypeKind.Etkinlik);

            if (!etkinlikPageTypeResult.IsSuccess || etkinlikPageTypeResult.Data == null)
                return ServiceResult<object>.Error(
                    etkinlikPageTypeResult.Fail?.Detail ??
                    etkinlikPageTypeResult.Fail?.Title ??
                    "Etkinlik sayfa türü bulunamadı");

            // PageTypeId'yi client'tan değil server'dan belirle
            dto.PageTypeId = etkinlikPageTypeResult.Data.Id;

            // SEO bilgileri kullanıcıdan alınmaz; başlıktan otomatik üretilir
            await _seoService.ApplyAutoSeoAsync(dto.SiteId, dto.PageTypeId, dto.Baslik, dto.KisaAciklama,
                seoUrl => dto.SeoUrl = seoUrl,
                seoTitle => dto.SeoTitle = seoTitle,
                seoDescription => dto.SeoDescription = seoDescription,
                fallbackSlug: "etkinlik",
                excludeIcerikId: dto.Id);








            _logger.LogInformation("Etkinlik güncelleniyor. Id: {Id}", dto.Id);
            var response = await _etkinlikClient.UpdateEtkinlikAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Etkinlik güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Etkinlik güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteEtkinlikAsync(int id)
        {
            _logger.LogWarning("Etkinlik silme isteği alındı. Id: {Id}", id);
            var response = await _etkinlikClient.DeleteEtkinlikAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Etkinlik silinemedi");
            }

            _logger.LogInformation("Etkinlik silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<PaginatedResult<GetEtkinlikVm>>> GetEtkinliklerPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir)
        {
            _logger.LogInformation("Paginated etkinlik listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            var response = await _etkinlikClient.GetEtkinliklerPaginatedAsync(siteId, dilId, page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}", response.StatusCode, problemDetails?.Title);
                return ServiceResult<PaginatedResult<GetEtkinlikVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated etkinlik listesi alınamadı");
            }

            return ServiceResult<PaginatedResult<GetEtkinlikVm>>.Success(response.Content!);
        }
    }
}