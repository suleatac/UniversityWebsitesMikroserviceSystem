using Microservice.Admin.Clients.GaleriResimClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.GaleriResim;
using Microservice.Admin.ViewModels.PageType;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class GaleriResimService : IGaleriResimService
    {
        private readonly IGaleriResimClientServices _galeriResimClient;
        private readonly ILogger<GaleriResimService> _logger;
        private readonly IPageTypeService _pageTypeService;
        private readonly ISiteService _siteService;
        private readonly ISeoService _seoService;

        public GaleriResimService(
            IGaleriResimClientServices galeriResimClient,
            ILogger<GaleriResimService> logger,
            IPageTypeService pageTypeService,
            ISiteService siteService,
            ISeoService seoService)
        {
            _galeriResimClient = galeriResimClient ?? throw new ArgumentNullException(nameof(galeriResimClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pageTypeService = pageTypeService ?? throw new ArgumentNullException(nameof(pageTypeService));
            _siteService = siteService ?? throw new ArgumentNullException(nameof(siteService));
            _seoService = seoService ?? throw new ArgumentNullException(nameof(seoService));
        }

        public async Task<ServiceResult<List<GetGaleriResimVm>>> GetGaleriResimlerAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Galeri resimi listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _galeriResimClient.GetGaleriResimlerAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetGaleriResimVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Galeri resimleri alınamadı");
            }

            return ServiceResult<List<GetGaleriResimVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<GaleriResimDetailVm>> GetGaleriResimByIdAsync(int id)
        {
            _logger.LogInformation("Galeri resimi getiriliyor. Id: {Id}", id);
            var response = await _galeriResimClient.GetGaleriResimByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<GaleriResimDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Galeri resimi bulunamadı");
            }

            return ServiceResult<GaleriResimDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreateGaleriResimAsync(CreateGaleriResimVm dto)
        {
            var siteResult = await _siteService.GetSiteByIdAsync(dto.SiteId);

            if (!siteResult.IsSuccess || siteResult.Data == null)
                return ServiceResult<object>.Error(
                    siteResult.Fail?.Detail ??
                    siteResult.Fail?.Title ??
                    "Site bilgisi alınamadı");

            var galeriResimPageTypeResult =
                await _pageTypeService.GetPageTypeByTemplateIdAndPageTypeKindAsync(
                    siteResult.Data.TemplateId,
                    dto.DilId,
                    PageTypeKind.GaleriResimDetay);

            if (!galeriResimPageTypeResult.IsSuccess || galeriResimPageTypeResult.Data == null)
                return ServiceResult<object>.Error(
                    galeriResimPageTypeResult.Fail?.Detail ??
                    galeriResimPageTypeResult.Fail?.Title ??
                    "Galeri resmi sayfa türü bulunamadı");

            // PageTypeId'yi client'tan değil server'dan belirle
            dto.PageTypeId = galeriResimPageTypeResult.Data.Id;

            // SEO bilgileri kullanıcıdan alınmaz; başlıktan otomatik üretilir
            await _seoService.ApplyAutoSeoAsync(dto.SiteId, dto.PageTypeId, dto.Baslik, dto.KisaAciklama,
                seoUrl => dto.SeoUrl = seoUrl,
                seoTitle => dto.SeoTitle = seoTitle,
                seoDescription => dto.SeoDescription = seoDescription,
                fallbackSlug: "galeri-resmi");

            _logger.LogInformation("Yeni galeri resimi oluşturuluyor. Başlık: {Title}", dto.Baslik);
            var response = await _galeriResimClient.CreateGaleriResimAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Galeri resimi oluşturulamadı");
            }

            _logger.LogInformation("Galeri resimi oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdateGaleriResimAsync(GaleriResimDetailVm dto)
        {
            var siteResult = await _siteService.GetSiteByIdAsync(dto.SiteId);

            if (!siteResult.IsSuccess || siteResult.Data == null)
                return ServiceResult<object>.Error(
                    siteResult.Fail?.Detail ??
                    siteResult.Fail?.Title ??
                    "Site bilgisi alınamadı");

            var galeriResimPageTypeResult =
                await _pageTypeService.GetPageTypeByTemplateIdAndPageTypeKindAsync(
                    siteResult.Data.TemplateId,
                    dto.DilId,
                    PageTypeKind.GaleriResimDetay);

            if (!galeriResimPageTypeResult.IsSuccess || galeriResimPageTypeResult.Data == null)
                return ServiceResult<object>.Error(
                    galeriResimPageTypeResult.Fail?.Detail ??
                    galeriResimPageTypeResult.Fail?.Title ??
                    "Galeri resmi sayfa türü bulunamadı");

            // PageTypeId'yi client'tan değil server'dan belirle
            dto.PageTypeId = galeriResimPageTypeResult.Data.Id;

            // SEO bilgileri kullanıcıdan alınmaz; başlıktan otomatik üretilir
            await _seoService.ApplyAutoSeoAsync(dto.SiteId, dto.PageTypeId, dto.Baslik, dto.KisaAciklama,
                seoUrl => dto.SeoUrl = seoUrl,
                seoTitle => dto.SeoTitle = seoTitle,
                seoDescription => dto.SeoDescription = seoDescription,
                fallbackSlug: "galeri-resmi",
                excludeIcerikId: dto.Id);

            _logger.LogInformation("Galeri resimi güncelleniyor. Id: {Id}", dto.Id);
            var response = await _galeriResimClient.UpdateGaleriResimAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Galeri resimi güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Galeri resimi güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteGaleriResimAsync(int id)
        {
            _logger.LogWarning("Galeri resimi silme isteği alındı. Id: {Id}", id);
            var response = await _galeriResimClient.DeleteGaleriResimAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Galeri resimi silinemedi");
            }

            _logger.LogInformation("Galeri resimi silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<PaginatedResult<GetGaleriResimVm>>> GetGaleriResimlerPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir)
        {
            _logger.LogInformation("Paginated galeri resimi listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            var response = await _galeriResimClient.GetGaleriResimlerPaginatedAsync(siteId, dilId, page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}", response.StatusCode, problemDetails?.Title);
                return ServiceResult<PaginatedResult<GetGaleriResimVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated galeri resimi listesi alınamadı");
            }

            return ServiceResult<PaginatedResult<GetGaleriResimVm>>.Success(response.Content!);
        }
    }
}
