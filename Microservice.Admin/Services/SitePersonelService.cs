using Microservice.Admin.Clients.SitePersonelClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.PageType;
using Microservice.Admin.ViewModels.SitePersonel;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class SitePersonelService : ISitePersonelService
    {
        private readonly ISitePersonelClientServices _sitePersonelClient;
        private readonly ILogger<SitePersonelService> _logger;
        private readonly IPageTypeService _pageTypeService;
        private readonly ISiteService _siteService;
        private readonly ISeoService _seoService;
        private readonly IPersonelService _personelService;
        public SitePersonelService(
            ISitePersonelClientServices sitePersonelClient, 
            ILogger<SitePersonelService> logger,
            IPageTypeService pageTypeService,
            ISiteService siteService,
            ISeoService seoService,
            IPersonelService personelService
            )
        {
            _sitePersonelClient = sitePersonelClient ?? throw new ArgumentNullException(nameof(sitePersonelClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pageTypeService = pageTypeService ?? throw new ArgumentNullException(nameof(pageTypeService));
            _siteService = siteService ?? throw new ArgumentNullException(nameof(siteService));
            _seoService = seoService ?? throw new ArgumentNullException(nameof(seoService));
            _personelService = personelService ?? throw new ArgumentNullException(nameof(personelService));
        }

        public async Task<ServiceResult<List<GetSitePersonelVm>>> GetSitePersonellerAsync(int siteId)
        {
            _logger.LogInformation("Site personel listesi çekiliyor. SiteId: {SiteId}", siteId);
            var response = await _sitePersonelClient.GetSitePersonellerAsync(siteId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetSitePersonelVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Site personelleri alınamadı");
            }

            return ServiceResult<List<GetSitePersonelVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<SitePersonelDetailVm>> GetSitePersonelByIdAsync(int id)
        {
            _logger.LogInformation("Site personel getiriliyor. Id: {Id}", id);
            var response = await _sitePersonelClient.GetSitePersonelByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<SitePersonelDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Site personel bulunamadı");
            }

            return ServiceResult<SitePersonelDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreateSitePersonelAsync(CreateSitePersonelVm dto)
        {




            var siteResult = await _siteService.GetSiteByIdAsync(dto.SiteId);

            if (!siteResult.IsSuccess || siteResult.Data == null)
                return ServiceResult<object>.Error(
                    siteResult.Fail?.Detail ??
                    siteResult.Fail?.Title ??
                    "Site bilgisi alınamadı");

            var personelPageTypeResult =
                await _pageTypeService.GetPageTypeByTemplateIdAndPageTypeKindAsync(
                    siteResult.Data.TemplateId,
                    siteResult.Data.DefaultLanguageId,
                    PageTypeKind.PersonelDetay);

            if (!personelPageTypeResult.IsSuccess || personelPageTypeResult.Data == null)
                return ServiceResult<object>.Error(
                    personelPageTypeResult.Fail?.Detail ??
                    personelPageTypeResult.Fail?.Title ??
                    "Personel sayfa türü bulunamadı");

            // PageTypeId'yi client'tan değil server'dan belirle
            dto.PageTypeId = personelPageTypeResult.Data.Id;

            // SEO bilgileri kullanıcıdan alınmaz; başlıktan otomatik üretilir
            await _seoService.ApplyAutoSeoAsync(dto.SiteId, dto.PageTypeId, dto.Adi+dto.Soyadi, dto.Username,
                seoUrl => dto.SeoUrl = seoUrl,
                seoTitle => dto.SeoTitle = seoTitle,
                seoDescription => dto.SeoDescription = seoDescription,
                fallbackSlug: "personel");
            // Site Personeli teyit amaçlı tekrar apiden çekilir ve 3 alan (Adi, Soyadi, Username) client tarafında güncellenir. Bu sayede kullanıcı yanlışlıkla farklı bir personel seçse bile doğru bilgilerle kayıt yapılır.
            var personel = await _personelService.GetPersonelByIdAsync(dto.PersonelId);

            if (!personel.IsSuccess || personel.Data == null)
                return ServiceResult<object>.Error(
                    personel.Fail?.Detail ??
                    personel.Fail?.Title ??
                    "Personel bilgisi alınamadı");

            dto.Adi = personel.Data.Adi;
            dto.Soyadi = personel.Data.Soyadi;
            dto.Username = personel.Data.Username;

            _logger.LogInformation("Yeni site personel oluşturuluyor.");
            var response = await _sitePersonelClient.CreateSitePersonelAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Site personel oluşturulamadı");
            }

            _logger.LogInformation("Site personel oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdateSitePersonelAsync(SitePersonelDetailVm dto)
        {



            var siteResult = await _siteService.GetSiteByIdAsync(dto.SiteId);

            if (!siteResult.IsSuccess || siteResult.Data == null)
                return ServiceResult<object>.Error(
                    siteResult.Fail?.Detail ??
                    siteResult.Fail?.Title ??
                    "Site bilgisi alınamadı");

            var personelPageTypeResult =
                await _pageTypeService.GetPageTypeByTemplateIdAndPageTypeKindAsync(
                    siteResult.Data.TemplateId,
                    siteResult.Data.DefaultLanguageId,
                    PageTypeKind.PersonelDetay);

            if (!personelPageTypeResult.IsSuccess || personelPageTypeResult.Data == null)
                return ServiceResult<object>.Error(
                    personelPageTypeResult.Fail?.Detail ??
                    personelPageTypeResult.Fail?.Title ??
                    "Personel sayfa türü bulunamadı");

            // PageTypeId'yi client'tan değil server'dan belirle
            dto.PageTypeId = personelPageTypeResult.Data.Id;


            // SEO bilgileri kullanıcıdan alınmaz; başlıktan otomatik üretilir
            await _seoService.ApplyAutoSeoAsync(dto.SiteId, dto.PageTypeId, dto.Adi + dto.Soyadi, dto.Username,
                seoUrl => dto.SeoUrl = seoUrl,
                seoTitle => dto.SeoTitle = seoTitle,
                seoDescription => dto.SeoDescription = seoDescription,
                fallbackSlug: "personel");


            // Site Personeli teyit amaçlı tekrar apiden çekilir ve 3 alan (Adi, Soyadi, Username) client tarafında güncellenir. Bu sayede kullanıcı yanlışlıkla farklı bir personel seçse bile doğru bilgilerle kayıt yapılır.
            var personel = await _personelService.GetPersonelByIdAsync(dto.PersonelId);

            if (!personel.IsSuccess || personel.Data == null)
                return ServiceResult<object>.Error(
                    personel.Fail?.Detail ??
                    personel.Fail?.Title ??
                    "Personel bilgisi alınamadı");

            dto.Adi = personel.Data.Adi;
            dto.Soyadi = personel.Data.Soyadi;
            dto.Username = personel.Data.Username;




            _logger.LogInformation("Site personel güncelleniyor. Id: {Id}", dto.Id);
            var response = await _sitePersonelClient.UpdateSitePersonelAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Site personel güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Site personel güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteSitePersonelAsync(int id)
        {
            _logger.LogWarning("Site personel silme isteği alındı. Id: {Id}", id);
            var response = await _sitePersonelClient.DeleteSitePersonelAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Site personel silinemedi");
            }

            _logger.LogInformation("Site personel silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }
    }
}